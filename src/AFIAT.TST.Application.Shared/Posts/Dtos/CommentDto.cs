using System;
using Abp.Application.Services.Dto;

namespace AFIAT.TST.Posts.Dtos
{
    public class CommentDto : EntityDto
    {
        public string Description { get; set; }

        public double? Likes { get; set; }

        public int ItemId { get; set; }

    }
}