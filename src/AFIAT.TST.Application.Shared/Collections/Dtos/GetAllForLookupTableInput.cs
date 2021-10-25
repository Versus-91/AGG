using Abp.Application.Services.Dto;

namespace AFIAT.TST.Collections.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}