using AFIAT.TST.Collections;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace AFIAT.TST.SubCollections
{
    [Table("SubCollections")]
    public class SubCollection : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Title { get; set; }

        public virtual string Description { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual int? CollectionId { get; set; }

        [ForeignKey("CollectionId")]
        public Collection CollectionFk { get; set; }

    }
}