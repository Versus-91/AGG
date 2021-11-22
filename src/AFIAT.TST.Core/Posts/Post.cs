using AFIAT.TST.Pages;
using AFIAT.TST.Posts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace AFIAT.TST.Posts
{
    [Table("Posts")]
    public class Post : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Title { get; set; }

        public virtual string Description { get; set; }

        public virtual string IsActive { get; set; }

        public virtual string KeyWords { get; set; }

        public virtual int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item ItemFk { get; set; }

        public virtual int? PostTypesId { get; set; }

        [ForeignKey("PostTypesId")]
        public PostTypes PostTypesFk { get; set; }

    }
}