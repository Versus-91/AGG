using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AFIAT.TST.Pages.Dtos;
using AFIAT.TST.Dto;

namespace AFIAT.TST.Pages
{
    public interface ICategoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCategoryForViewDto>> GetAll(GetAllCategoriesInput input);

        Task<GetCategoryForViewDto> GetCategoryForView(int id);

        Task<GetCategoryForEditOutput> GetCategoryForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCategoryDto input);

        Task Delete(EntityDto input);

    }
}