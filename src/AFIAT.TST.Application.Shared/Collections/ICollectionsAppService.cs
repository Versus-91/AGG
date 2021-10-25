using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AFIAT.TST.Collections.Dtos;
using AFIAT.TST.Dto;

namespace AFIAT.TST.Collections
{
    public interface ICollectionsAppService : IApplicationService
    {
        Task<PagedResultDto<GetCollectionForViewDto>> GetAll(GetAllCollectionsInput input);

        Task<GetCollectionForViewDto> GetCollectionForView(int id);

        Task<GetCollectionForEditOutput> GetCollectionForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCollectionDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetCollectionsToExcel(GetAllCollectionsForExcelInput input);

    }
}