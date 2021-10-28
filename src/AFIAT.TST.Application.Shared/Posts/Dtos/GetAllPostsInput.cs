using Abp.Application.Services.Dto;
using System;

namespace AFIAT.TST.Posts.Dtos
{
    public class GetAllPostsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string TitleFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string IsActiveFilter { get; set; }

        public string ItemTitleFilter { get; set; }

    }
}