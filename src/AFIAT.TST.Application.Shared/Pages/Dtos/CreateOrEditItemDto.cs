using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AFIAT.TST.Pages.Dtos
{
    public class CreateOrEditItemDto : EntityDto<int?>
    {

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public string ImageAdress { get; set; }

        public string VideoAddress { get; set; }

        public int CollectionId { get; set; }

    }
}