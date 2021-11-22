using Abp.Application.Services.Dto;
using System;

namespace AFIAT.TST.Posts.Dtos
{
    public class GetAllPostTypesesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public int? IsActiveFilter { get; set; }

        public string DescriptionFilter { get; set; }

    }
}