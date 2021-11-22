using System;
using Abp.Application.Services.Dto;

namespace AFIAT.TST.Posts.Dtos
{
    public class PostDto : EntityDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string IsActive { get; set; }

        public string KeyWords { get; set; }

        public int ItemId { get; set; }

        public int? PostTypesId { get; set; }

    }
}