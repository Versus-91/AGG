using System;
using Abp.Application.Services.Dto;

namespace AFIAT.TST.Posts.Dtos
{
    public class PostDto : EntityDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string IsActive { get; set; }

        public int ItemId { get; set; }

    }
}