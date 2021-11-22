using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AFIAT.TST.Posts.Dtos
{
    public class CreateOrEditPostDto : EntityDto<int?>
    {

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public string IsActive { get; set; }

        public string KeyWords { get; set; }

        public int ItemId { get; set; }

        public int? PostTypesId { get; set; }

    }
}