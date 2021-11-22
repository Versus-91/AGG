using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AFIAT.TST.Pages.Dtos;
using AFIAT.TST.Dto;

namespace AFIAT.TST.Pages
{
    public interface IItemsAppService : IApplicationService
    {
        Task<PagedResultDto<GetItemForViewDto>> GetAll(GetAllItemsInput input);

        Task<GetItemForViewDto> GetItemForView(int id);

        Task<GetItemForEditOutput> GetItemForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditItemDto input);

        Task Delete(EntityDto input);

        Task<PagedResultDto<ItemSubCollectionLookupTableDto>> GetAllSubCollectionForLookupTable(GetAllForLookupTableInput input);
        Task<GetItemForViewDto> GetPageByTitle(string title);

    }
}