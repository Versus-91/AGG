using Abp.Application.Services.Dto;
using System;

namespace AFIAT.TST.SubCollections.Dtos
{
    public class GetAllSubCollectionsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string TitleFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? IsActiveFilter { get; set; }

        public string CollectionNameFilter { get; set; }

    }
}