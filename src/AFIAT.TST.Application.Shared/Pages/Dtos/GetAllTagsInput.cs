using Abp.Application.Services.Dto;
using System;

namespace AFIAT.TST.Pages.Dtos
{
    public class GetAllTagsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string nameFilter { get; set; }

    }
}