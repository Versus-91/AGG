using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.MultiTenancy;

namespace AFIAT.TST.MultiTenancy.Payments
{
    [Table("AppSubscriptionPaymentsExtensionData")]
    [MultiTenancySide(MultiTenancySides.Host)]
    public class SubscriptionPaymentExtensionData : Entity<long>, ISoftDelete
    {
        public long SubscriptionPaymentId { get; set; }

        [StringLength(700)] // Explicit data length. Result data type is nvarchar(25)
        public string Key { get; set; }

        public string Value { get; set; }

        public bool IsDeleted { get; set; }
    }
}
