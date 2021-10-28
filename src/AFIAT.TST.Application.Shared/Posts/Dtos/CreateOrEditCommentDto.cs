using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AFIAT.TST.Posts.Dtos
{
    public class CreateOrEditCommentDto : EntityDto<int?>
    {

        public string Description { get; set; }

        public double? Likes { get; set; }

        public int ItemId { get; set; }

    }
}