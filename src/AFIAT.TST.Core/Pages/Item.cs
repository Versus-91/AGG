using AFIAT.TST.SubCollections;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace AFIAT.TST.Pages
{
    [Table("Items")]
    public class Item : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Title { get; set; }

        public virtual string Description { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual string ImageAdress { get; set; }

        public virtual string VideoAddress { get; set; }

        public virtual int SubCollectionId { get; set; }

        [ForeignKey("SubCollectionId")]
        public SubCollection SubCollectionFk { get; set; }

    }
}