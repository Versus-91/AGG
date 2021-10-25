using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AFIAT.TST.SubCollections.Dtos
{
    public class CreateOrEditSubCollectionDto : EntityDto<int?>
    {

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int? CollectionId { get; set; }

    }
}