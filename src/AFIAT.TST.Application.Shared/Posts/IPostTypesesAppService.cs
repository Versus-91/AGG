using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AFIAT.TST.Posts.Dtos;
using AFIAT.TST.Dto;

namespace AFIAT.TST.Posts
{
    public interface IPostTypesesAppService : IApplicationService
    {
        Task<PagedResultDto<GetPostTypesForViewDto>> GetAll(GetAllPostTypesesInput input);

        Task<GetPostTypesForViewDto> GetPostTypesForView(int id);

        Task<GetPostTypesForEditOutput> GetPostTypesForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditPostTypesDto input);

        Task Delete(EntityDto input);

    }
}