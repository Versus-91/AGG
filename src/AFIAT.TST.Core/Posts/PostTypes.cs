using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace AFIAT.TST.Posts
{
    [Table("PostTypeses")]
    public class PostTypes : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual string Description { get; set; }

    }
}