using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AFIAT.TST.Collections.Dtos
{
    public class GetCollectionForEditOutput
    {
        public CreateOrEditCollectionDto Collection { get; set; }

    }
}