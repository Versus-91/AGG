using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace AFIAT.TST.Pages
{
    [Table("Tags")]
    public class Tag : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string name { get; set; }

    }
}