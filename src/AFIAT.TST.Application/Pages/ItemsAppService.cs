using AFIAT.TST.SubCollections;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using AFIAT.TST.Pages.Dtos;
using AFIAT.TST.Dto;
using Abp.Application.Services.Dto;
using AFIAT.TST.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using AFIAT.TST.Storage;
using Abp.Runtime.Caching;

namespace AFIAT.TST.Pages
{
    //[AbpAuthorize(AppPermissions.Pages_Items)]
    public class ItemsAppService : TSTAppServiceBase, IItemsAppService
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly IRepository<SubCollection, int> _lookup_subCollectionRepository;

        public ItemsAppService(IRepository<Item> itemRepository, IRepository<SubCollection, int> lookup_subCollectionRepository)
        {
            _itemRepository = itemRepository;
            _lookup_subCollectionRepository = lookup_subCollectionRepository;
        }

        public async Task<PagedResultDto<GetItemForViewDto>> GetAll(GetAllItemsInput input)
        {

            var filteredItems = _itemRepository.GetAll()
                        .Include(e => e.SubCollectionFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.ImageAdress.Contains(input.Filter) || e.VideoAddress.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ImageAdressFilter), e => e.ImageAdress == input.ImageAdressFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VideoAddressFilter), e => e.VideoAddress == input.VideoAddressFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubCollectionTitleFilter), e => e.SubCollectionFk != null && e.SubCollectionFk.Title == input.SubCollectionTitleFilter);

            var pagedAndFilteredItems = filteredItems
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var items = from o in pagedAndFilteredItems
                        join o1 in _lookup_subCollectionRepository.GetAll() on o.SubCollectionId equals o1.Id into j1
                        from s1 in j1.DefaultIfEmpty()

                        select new
                        {

                            o.Title,
                            o.Description,
                            o.IsActive,
                            o.ImageAdress,
                            o.VideoAddress,
                            Id = o.Id,
                            SubCollectionTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString()
                        };

            var totalCount = await filteredItems.CountAsync();

            var dbList = await items.ToListAsync();
            var results = new List<GetItemForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetItemForViewDto()
                {
                    Item = new ItemDto
                    {

                        Title = o.Title,
                        Description = o.Description,
                        IsActive = o.IsActive,
                        ImageAdress = o.ImageAdress,
                        VideoAddress = o.VideoAddress,
                        Id = o.Id,
                    },
                    SubCollectionTitle = o.SubCollectionTitle
                };

                results.Add(res);
            }

            return new PagedResultDto<GetItemForViewDto>(
                totalCount,
                results
            );

        }
        public async Task<GetItemForViewDto> GetPageByTitle(string title)
        {
            var item = await _itemRepository.FirstOrDefaultAsync(m => m.Title.Equals(title));
            var output = new GetItemForViewDto { Item = ObjectMapper.Map<ItemDto>(item) };
            return output;
        }
        public async Task<GetItemForViewDto> GetItemForView(int id)
        {
            var item = await _itemRepository.GetAsync(id);

            var output = new GetItemForViewDto { Item = ObjectMapper.Map<ItemDto>(item) };

            if (output.Item.SubCollectionId != null)
            {
                var _lookupSubCollection = await _lookup_subCollectionRepository.FirstOrDefaultAsync((int)output.Item.SubCollectionId);
                output.SubCollectionTitle = _lookupSubCollection?.Title?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Items_Edit)]
        public async Task<GetItemForEditOutput> GetItemForEdit(EntityDto input)
        {
            var item = await _itemRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetItemForEditOutput { Item = ObjectMapper.Map<CreateOrEditItemDto>(item) };

            if (output.Item.SubCollectionId != null)
            {
                var _lookupSubCollection = await _lookup_subCollectionRepository.FirstOrDefaultAsync((int)output.Item.SubCollectionId);
                output.SubCollectionTitle = _lookupSubCollection?.Title?.ToString();
            }

            return output;
        }
        [AbpAuthorize(AppPermissions.Pages_Items_Edit)]
        public async Task CreateOrEdit(CreateOrEditItemDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Items_Create)]
        protected virtual async Task Create(CreateOrEditItemDto input)
        {
            var item = ObjectMapper.Map<Item>(input);

            if (AbpSession.TenantId != null)
            {
                item.TenantId = (int?)AbpSession.TenantId;
            }

            await _itemRepository.InsertAsync(item);

        }

        [AbpAuthorize(AppPermissions.Pages_Items_Edit)]
        protected virtual async Task Update(CreateOrEditItemDto input)
        {
            var item = await _itemRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, item);

        }

        [AbpAuthorize(AppPermissions.Pages_Items_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _itemRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Items)]
        public async Task<PagedResultDto<ItemSubCollectionLookupTableDto>> GetAllSubCollectionForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_subCollectionRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Title != null && e.Title.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var subCollectionList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ItemSubCollectionLookupTableDto>();
            foreach (var subCollection in subCollectionList)
            {
                lookupTableDtoList.Add(new ItemSubCollectionLookupTableDto
                {
                    Id = subCollection.Id,
                    DisplayName = subCollection.Title?.ToString()
                });
            }

            return new PagedResultDto<ItemSubCollectionLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}