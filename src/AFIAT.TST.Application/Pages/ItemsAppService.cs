using AFIAT.TST.Collections;

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
        private readonly IRepository<Collection, int> _lookup_collectionRepository;
        private readonly ICacheManager _cacheManager;

        public ItemsAppService(IRepository<Item> itemRepository, IRepository<Collection, int> lookup_collectionRepository, ICacheManager cacheManager)
        {
            _itemRepository = itemRepository;
            _cacheManager = cacheManager;
            _lookup_collectionRepository = lookup_collectionRepository;

        }
        public async Task<GetItemForViewDto> GetPageByTitle(string title)
        {
            var item = await _cacheManager
               .GetCache("pages")
               .GetAsync(title, async (title) => await GetPageByTitleFromDatabase(title)) as GetItemForViewDto;
            var output = new GetItemForViewDto { Item = ObjectMapper.Map<ItemDto>(item) };
            return output;
        }
        private async Task<GetItemForViewDto> GetPageByTitleFromDatabase(string title)
        {
            var item = await _itemRepository.FirstOrDefaultAsync(m => m.Title.Equals(title));
            var output = new GetItemForViewDto { Item = ObjectMapper.Map<ItemDto>(item) };
            return  output;
        }
        public async Task<PagedResultDto<GetItemForViewDto>> GetAll(GetAllItemsInput input)
        {

            var filteredItems = _itemRepository.GetAll()
                        .Include(e => e.CollectionFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.ImageAdress.Contains(input.Filter) || e.VideoAddress.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ImageAdressFilter), e => e.ImageAdress == input.ImageAdressFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VideoAddressFilter), e => e.VideoAddress == input.VideoAddressFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CollectionNameFilter), e => e.CollectionFk != null && e.CollectionFk.Name == input.CollectionNameFilter);

            var pagedAndFilteredItems = filteredItems
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var items = from o in pagedAndFilteredItems
                        join o1 in _lookup_collectionRepository.GetAll() on o.CollectionId equals o1.Id into j1
                        from s1 in j1.DefaultIfEmpty()

                        select new
                        {

                            o.Title,
                            o.Description,
                            o.IsActive,
                            o.ImageAdress,
                            o.VideoAddress,
                            Id = o.Id,
                            CollectionName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
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
                    CollectionName = o.CollectionName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetItemForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetItemForViewDto> GetItemForView(int id)
        {
            var item = await _itemRepository.GetAsync(id);

            var output = new GetItemForViewDto { Item = ObjectMapper.Map<ItemDto>(item) };

            if (output.Item.CollectionId != null)
            {
                var _lookupCollection = await _lookup_collectionRepository.FirstOrDefaultAsync((int)output.Item.CollectionId);
                output.CollectionName = _lookupCollection?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Items_Edit)]
        public async Task<GetItemForEditOutput> GetItemForEdit(EntityDto input)
        {
            var item = await _itemRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetItemForEditOutput { Item = ObjectMapper.Map<CreateOrEditItemDto>(item) };

            if (output.Item.CollectionId != null)
            {
                var _lookupCollection = await _lookup_collectionRepository.FirstOrDefaultAsync((int)output.Item.CollectionId);
                output.CollectionName = _lookupCollection?.Name?.ToString();
            }

            return output;
        }

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
        public async Task<PagedResultDto<ItemCollectionLookupTableDto>> GetAllCollectionForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_collectionRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var collectionList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ItemCollectionLookupTableDto>();
            foreach (var collection in collectionList)
            {
                lookupTableDtoList.Add(new ItemCollectionLookupTableDto
                {
                    Id = collection.Id,
                    DisplayName = collection.Name?.ToString()
                });
            }

            return new PagedResultDto<ItemCollectionLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}