using Abp.Application.Services.Dto;
using System;

namespace AFIAT.TST.Posts.Dtos
{
    public class GetAllCommentsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string DescriptionFilter { get; set; }

        public double? MaxLikesFilter { get; set; }
        public double? MinLikesFilter { get; set; }

        public string ItemTitleFilter { get; set; }

    }
}