using AFIAT.TST.Pages;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace AFIAT.TST.Posts
{
    [Table("Comments")]
    public class Comment : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Description { get; set; }

        public virtual double? Likes { get; set; }

        public virtual int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item ItemFk { get; set; }

    }
}