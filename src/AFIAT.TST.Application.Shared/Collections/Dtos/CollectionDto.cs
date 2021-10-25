using System;
using Abp.Application.Services.Dto;

namespace AFIAT.TST.Collections.Dtos
{
    public class CollectionDto : EntityDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int CountOfItems { get; set; }

    }
}