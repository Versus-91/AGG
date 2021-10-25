using AFIAT.TST.Collections;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using AFIAT.TST.SubCollections.Dtos;
using AFIAT.TST.Dto;
using Abp.Application.Services.Dto;
using AFIAT.TST.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using AFIAT.TST.Storage;

namespace AFIAT.TST.SubCollections
{
    [AbpAuthorize(AppPermissions.Pages_SubCollections)]
    public class SubCollectionsAppService : TSTAppServiceBase, ISubCollectionsAppService
    {
        private readonly IRepository<SubCollection> _subCollectionRepository;
        private readonly IRepository<Collection, int> _lookup_collectionRepository;

        public SubCollectionsAppService(IRepository<SubCollection> subCollectionRepository, IRepository<Collection, int> lookup_collectionRepository)
        {
            _subCollectionRepository = subCollectionRepository;
            _lookup_collectionRepository = lookup_collectionRepository;

        }

        public async Task<PagedResultDto<GetSubCollectionForViewDto>> GetAll(GetAllSubCollectionsInput input)
        {

            var filteredSubCollections = _subCollectionRepository.GetAll()
                        .Include(e => e.CollectionFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CollectionNameFilter), e => e.CollectionFk != null && e.CollectionFk.Name == input.CollectionNameFilter);

            var pagedAndFilteredSubCollections = filteredSubCollections
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var subCollections = from o in pagedAndFilteredSubCollections
                                 join o1 in _lookup_collectionRepository.GetAll() on o.CollectionId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()

                                 select new
                                 {

                                     o.Title,
                                     o.Description,
                                     o.IsActive,
                                     Id = o.Id,
                                     CollectionName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                 };

            var totalCount = await filteredSubCollections.CountAsync();

            var dbList = await subCollections.ToListAsync();
            var results = new List<GetSubCollectionForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSubCollectionForViewDto()
                {
                    SubCollection = new SubCollectionDto
                    {

                        Title = o.Title,
                        Description = o.Description,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    },
                    CollectionName = o.CollectionName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetSubCollectionForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetSubCollectionForViewDto> GetSubCollectionForView(int id)
        {
            var subCollection = await _subCollectionRepository.GetAsync(id);

            var output = new GetSubCollectionForViewDto { SubCollection = ObjectMapper.Map<SubCollectionDto>(subCollection) };

            if (output.SubCollection.CollectionId != null)
            {
                var _lookupCollection = await _lookup_collectionRepository.FirstOrDefaultAsync((int)output.SubCollection.CollectionId);
                output.CollectionName = _lookupCollection?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SubCollections_Edit)]
        public async Task<GetSubCollectionForEditOutput> GetSubCollectionForEdit(EntityDto input)
        {
            var subCollection = await _subCollectionRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSubCollectionForEditOutput { SubCollection = ObjectMapper.Map<CreateOrEditSubCollectionDto>(subCollection) };

            if (output.SubCollection.CollectionId != null)
            {
                var _lookupCollection = await _lookup_collectionRepository.FirstOrDefaultAsync((int)output.SubCollection.CollectionId);
                output.CollectionName = _lookupCollection?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSubCollectionDto input)
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

        [AbpAuthorize(AppPermissions.Pages_SubCollections_Create)]
        protected virtual async Task Create(CreateOrEditSubCollectionDto input)
        {
            var subCollection = ObjectMapper.Map<SubCollection>(input);

            if (AbpSession.TenantId != null)
            {
                subCollection.TenantId = (int?)AbpSession.TenantId;
            }

            await _subCollectionRepository.InsertAsync(subCollection);

        }

        [AbpAuthorize(AppPermissions.Pages_SubCollections_Edit)]
        protected virtual async Task Update(CreateOrEditSubCollectionDto input)
        {
            var subCollection = await _subCollectionRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, subCollection);

        }

        [AbpAuthorize(AppPermissions.Pages_SubCollections_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _subCollectionRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_SubCollections)]
        public async Task<PagedResultDto<SubCollectionCollectionLookupTableDto>> GetAllCollectionForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_collectionRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var collectionList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<SubCollectionCollectionLookupTableDto>();
            foreach (var collection in collectionList)
            {
                lookupTableDtoList.Add(new SubCollectionCollectionLookupTableDto
                {
                    Id = collection.Id,
                    DisplayName = collection.Name?.ToString()
                });
            }

            return new PagedResultDto<SubCollectionCollectionLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}