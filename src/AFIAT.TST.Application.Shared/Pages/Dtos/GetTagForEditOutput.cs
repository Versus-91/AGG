using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AFIAT.TST.Pages.Dtos
{
    public class GetTagForEditOutput
    {
        public CreateOrEditTagDto Tag { get; set; }

    }
}