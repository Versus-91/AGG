using Abp.Application.Services.Dto;
using System;

namespace AFIAT.TST.Pages.Dtos
{
    public class GetAllTagsForExcelInput
    {
        public string Filter { get; set; }

        public string nameFilter { get; set; }

    }
}