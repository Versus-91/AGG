using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AFIAT.TST.Pages.Dtos
{
    public class CreateOrEditCategoryDto : EntityDto<int?>
    {

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public string Image { get; set; }

        public string Video { get; set; }

    }
}