using Abp.Application.Services.Dto;
using System;

namespace AFIAT.TST.Pages.Dtos
{
    public class GetAllItemsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string TitleFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? IsActiveFilter { get; set; }

        public string ImageAdressFilter { get; set; }

        public string VideoAddressFilter { get; set; }

        public string SubCollectionTitleFilter { get; set; }

    }
}