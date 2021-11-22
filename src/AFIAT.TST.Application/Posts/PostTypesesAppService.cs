using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using AFIAT.TST.Posts.Dtos;
using AFIAT.TST.Dto;
using Abp.Application.Services.Dto;
using AFIAT.TST.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using AFIAT.TST.Storage;

namespace AFIAT.TST.Posts
{
    [AbpAuthorize(AppPermissions.Pages_PostTypeses)]
    public class PostTypesesAppService : TSTAppServiceBase, IPostTypesesAppService
    {
        private readonly IRepository<PostTypes> _postTypesRepository;

        public PostTypesesAppService(IRepository<PostTypes> postTypesRepository)
        {
            _postTypesRepository = postTypesRepository;

        }

        public async Task<PagedResultDto<GetPostTypesForViewDto>> GetAll(GetAllPostTypesesInput input)
        {

            var filteredPostTypeses = _postTypesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var pagedAndFilteredPostTypeses = filteredPostTypeses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var postTypeses = from o in pagedAndFilteredPostTypeses
                              select new
                              {

                                  o.Name,
                                  o.IsActive,
                                  o.Description,
                                  Id = o.Id
                              };

            var totalCount = await filteredPostTypeses.CountAsync();

            var dbList = await postTypeses.ToListAsync();
            var results = new List<GetPostTypesForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPostTypesForViewDto()
                {
                    PostTypes = new PostTypesDto
                    {

                        Name = o.Name,
                        IsActive = o.IsActive,
                        Description = o.Description,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetPostTypesForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetPostTypesForViewDto> GetPostTypesForView(int id)
        {
            var postTypes = await _postTypesRepository.GetAsync(id);

            var output = new GetPostTypesForViewDto { PostTypes = ObjectMapper.Map<PostTypesDto>(postTypes) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PostTypeses_Edit)]
        public async Task<GetPostTypesForEditOutput> GetPostTypesForEdit(EntityDto input)
        {
            var postTypes = await _postTypesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPostTypesForEditOutput { PostTypes = ObjectMapper.Map<CreateOrEditPostTypesDto>(postTypes) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPostTypesDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PostTypeses_Create)]
        protected virtual async Task Create(CreateOrEditPostTypesDto input)
        {
            var postTypes = ObjectMapper.Map<PostTypes>(input);

            if (AbpSession.TenantId != null)
            {
                postTypes.TenantId = (int?)AbpSession.TenantId;
            }

            await _postTypesRepository.InsertAsync(postTypes);

        }

        [AbpAuthorize(AppPermissions.Pages_PostTypeses_Edit)]
        protected virtual async Task Update(CreateOrEditPostTypesDto input)
        {
            var postTypes = await _postTypesRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, postTypes);

        }

        [AbpAuthorize(AppPermissions.Pages_PostTypeses_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _postTypesRepository.DeleteAsync(input.Id);
        }

    }
}