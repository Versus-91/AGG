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

namespace AFIAT.TST.Pages
{
    [AbpAuthorize(AppPermissions.Pages_Categories)]
    public class CategoriesAppService : TSTAppServiceBase, ICategoriesAppService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoriesAppService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;

        }

        public async Task<PagedResultDto<GetCategoryForViewDto>> GetAll(GetAllCategoriesInput input)
        {

            var filteredCategories = _categoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Image.Contains(input.Filter) || e.Video.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ImageFilter), e => e.Image == input.ImageFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VideoFilter), e => e.Video == input.VideoFilter);

            var pagedAndFilteredCategories = filteredCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var categories = from o in pagedAndFilteredCategories
                             select new
                             {

                                 o.Title,
                                 o.Description,
                                 o.IsActive,
                                 o.Image,
                                 o.Video,
                                 Id = o.Id
                             };

            var totalCount = await filteredCategories.CountAsync();

            var dbList = await categories.ToListAsync();
            var results = new List<GetCategoryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCategoryForViewDto()
                {
                    Category = new CategoryDto
                    {

                        Title = o.Title,
                        Description = o.Description,
                        IsActive = o.IsActive,
                        Image = o.Image,
                        Video = o.Video,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCategoryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCategoryForViewDto> GetCategoryForView(int id)
        {
            var category = await _categoryRepository.GetAsync(id);

            var output = new GetCategoryForViewDto { Category = ObjectMapper.Map<CategoryDto>(category) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Categories_Edit)]
        public async Task<GetCategoryForEditOutput> GetCategoryForEdit(EntityDto input)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCategoryForEditOutput { Category = ObjectMapper.Map<CreateOrEditCategoryDto>(category) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Categories_Create)]
        protected virtual async Task Create(CreateOrEditCategoryDto input)
        {
            var category = ObjectMapper.Map<Category>(input);

            if (AbpSession.TenantId != null)
            {
                category.TenantId = (int?)AbpSession.TenantId;
            }

            await _categoryRepository.InsertAsync(category);

        }

        [AbpAuthorize(AppPermissions.Pages_Categories_Edit)]
        protected virtual async Task Update(CreateOrEditCategoryDto input)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, category);

        }

        [AbpAuthorize(AppPermissions.Pages_Categories_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _categoryRepository.DeleteAsync(input.Id);
        }

    }
}