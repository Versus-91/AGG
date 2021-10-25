using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AFIAT.TST.Pages.Dtos
{
    public class CreateOrEditTagDto : EntityDto<int?>
    {

        public string name { get; set; }

    }
}