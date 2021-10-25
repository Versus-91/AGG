using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace AFIAT.TST.Collections
{
    [Table("Collections")]
    public class Collection : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual int CountOfItems { get; set; }

    }
}