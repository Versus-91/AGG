using Abp.Application.Services.Dto;
using System;

namespace AFIAT.TST.Pages.Dtos
{
    public class GetAllCategoriesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string TitleFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? IsActiveFilter { get; set; }

        public string ImageFilter { get; set; }

        public string VideoFilter { get; set; }

    }
}