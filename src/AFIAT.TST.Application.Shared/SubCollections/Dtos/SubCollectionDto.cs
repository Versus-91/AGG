using System;
using Abp.Application.Services.Dto;

namespace AFIAT.TST.SubCollections.Dtos
{
    public class SubCollectionDto : EntityDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int? CollectionId { get; set; }

    }
}