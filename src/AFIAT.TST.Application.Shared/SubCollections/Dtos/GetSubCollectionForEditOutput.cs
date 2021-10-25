using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AFIAT.TST.SubCollections.Dtos
{
    public class GetSubCollectionForEditOutput
    {
        public CreateOrEditSubCollectionDto SubCollection { get; set; }

        public string CollectionName { get; set; }

    }
}