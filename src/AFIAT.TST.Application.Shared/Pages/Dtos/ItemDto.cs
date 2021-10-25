using System;
using Abp.Application.Services.Dto;

namespace AFIAT.TST.Pages.Dtos
{
    public class ItemDto : EntityDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public string ImageAdress { get; set; }

        public string VideoAddress { get; set; }

        public int CollectionId { get; set; }

    }
}