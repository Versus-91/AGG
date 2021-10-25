using Abp.Application.Services.Dto;

namespace AFIAT.TST.SubCollections.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}