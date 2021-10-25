using System;
using Abp.Application.Services.Dto;

namespace AFIAT.TST.Pages.Dtos
{
    public class CategoryDto : EntityDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public string Image { get; set; }

        public string Video { get; set; }

    }
}