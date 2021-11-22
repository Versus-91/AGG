using System;
using Abp.Application.Services.Dto;

namespace AFIAT.TST.Posts.Dtos
{
    public class PostTypesDto : EntityDto
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }

    }
}