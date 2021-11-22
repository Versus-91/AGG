using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AFIAT.TST.Pages.Dtos
{
    public class GetItemForEditOutput
    {
        public CreateOrEditItemDto Item { get; set; }

        public string SubCollectionTitle { get; set; }

    }
}