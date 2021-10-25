using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AFIAT.TST.SubCollections.Dtos;
using AFIAT.TST.Dto;

namespace AFIAT.TST.SubCollections
{
    public interface ISubCollectionsAppService : IApplicationService
    {
        Task<PagedResultDto<GetSubCollectionForViewDto>> GetAll(GetAllSubCollectionsInput input);

        Task<GetSubCollectionForViewDto> GetSubCollectionForView(int id);

        Task<GetSubCollectionForEditOutput> GetSubCollectionForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditSubCollectionDto input);

        Task Delete(EntityDto input);

        Task<PagedResultDto<SubCollectionCollectionLookupTableDto>> GetAllCollectionForLookupTable(GetAllForLookupTableInput input);

    }
}