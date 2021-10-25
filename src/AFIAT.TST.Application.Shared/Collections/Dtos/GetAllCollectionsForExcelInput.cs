using Abp.Application.Services.Dto;
using System;

namespace AFIAT.TST.Collections.Dtos
{
    public class GetAllCollectionsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? IsActiveFilter { get; set; }

        public int? MaxCountOfItemsFilter { get; set; }
        public int? MinCountOfItemsFilter { get; set; }

    }
}