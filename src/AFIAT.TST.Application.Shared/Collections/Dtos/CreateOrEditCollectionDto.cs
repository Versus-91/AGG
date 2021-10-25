using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AFIAT.TST.Collections.Dtos
{
    public class CreateOrEditCollectionDto : EntityDto<int?>
    {

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int CountOfItems { get; set; }

    }
}