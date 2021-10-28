using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AFIAT.TST.Posts.Dtos
{
    public class GetCommentForEditOutput
    {
        public CreateOrEditCommentDto Comment { get; set; }

        public string ItemTitle { get; set; }

    }
}