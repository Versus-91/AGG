using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using AFIAT.TST.Collections.Exporting;
using AFIAT.TST.Collections.Dtos;
using AFIAT.TST.Dto;
using Abp.Application.Services.Dto;
using AFIAT.TST.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using AFIAT.TST.Storage;

namespace AFIAT.TST.Collections
{
    [AbpAuthorize(AppPermissions.Pages_Collections)]
    public class CollectionsAppService : TSTAppServiceBase, ICollectionsAppService
    {
        private readonly IRepository<Collection> _collectionRepository;
        private readonly ICollectionsExcelExporter _collectionsExcelExporter;

        public CollectionsAppService(IRepository<Collection> collectionRepository, ICollectionsExcelExporter collectionsExcelExporter)
        {
            _collectionRepository = collectionRepository;
            _collectionsExcelExporter = collectionsExcelExporter;

        }

        public async Task<PagedResultDto<GetCollectionForViewDto>> GetAll(GetAllCollectionsInput input)
        {

            var filteredCollections = _collectionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(input.MinCountOfItemsFilter != null, e => e.CountOfItems >= input.MinCountOfItemsFilter)
                        .WhereIf(input.MaxCountOfItemsFilter != null, e => e.CountOfItems <= input.MaxCountOfItemsFilter);

            var pagedAndFilteredCollections = filteredCollections
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var collections = from o in pagedAndFilteredCollections
                              select new
                              {

                                  o.Name,
                                  o.Description,
                                  o.IsActive,
                                  o.CountOfItems,
                                  Id = o.Id
                              };

            var totalCount = await filteredCollections.CountAsync();

            var dbList = await collections.ToListAsync();
            var results = new List<GetCollectionForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCollectionForViewDto()
                {
                    Collection = new CollectionDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        IsActive = o.IsActive,
                        CountOfItems = o.CountOfItems,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCollectionForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCollectionForViewDto> GetCollectionForView(int id)
        {
            var collection = await _collectionRepository.GetAsync(id);

            var output = new GetCollectionForViewDto { Collection = ObjectMapper.Map<CollectionDto>(collection) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Collections_Edit)]
        public async Task<GetCollectionForEditOutput> GetCollectionForEdit(EntityDto input)
        {
            var collection = await _collectionRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCollectionForEditOutput { Collection = ObjectMapper.Map<CreateOrEditCollectionDto>(collection) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCollectionDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Collections_Create)]
        protected virtual async Task Create(CreateOrEditCollectionDto input)
        {
            var collection = ObjectMapper.Map<Collection>(input);

            if (AbpSession.TenantId != null)
            {
                collection.TenantId = (int?)AbpSession.TenantId;
            }

            await _collectionRepository.InsertAsync(collection);

        }

        [AbpAuthorize(AppPermissions.Pages_Collections_Edit)]
        protected virtual async Task Update(CreateOrEditCollectionDto input)
        {
            var collection = await _collectionRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, collection);

        }

        [AbpAuthorize(AppPermissions.Pages_Collections_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _collectionRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCollectionsToExcel(GetAllCollectionsForExcelInput input)
        {

            var filteredCollections = _collectionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(input.MinCountOfItemsFilter != null, e => e.CountOfItems >= input.MinCountOfItemsFilter)
                        .WhereIf(input.MaxCountOfItemsFilter != null, e => e.CountOfItems <= input.MaxCountOfItemsFilter);

            var query = (from o in filteredCollections
                         select new GetCollectionForViewDto()
                         {
                             Collection = new CollectionDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 IsActive = o.IsActive,
                                 CountOfItems = o.CountOfItems,
                                 Id = o.Id
                             }
                         });

            var collectionListDtos = await query.ToListAsync();

            return _collectionsExcelExporter.ExportToFile(collectionListDtos);
        }

    }
}