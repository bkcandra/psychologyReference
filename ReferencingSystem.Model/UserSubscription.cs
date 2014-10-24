using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferencingSystem.Model
{
    public class UserSubscription
    {
        [Key]
        public string UserId { get; set; }

        public int subscriptionPlanId { get; set; }

        public int PaymentRecordId { get; set; }

        public int ExpiryUTC { get; set; }

        // record information

        public int CreatedUTC { get; set; }

        public string CreatedBy { get; set; }

        public int ModifiedUTC { get; set; }

        public string ModifiedBy { get; set; }

        [ForeignKey("subscriptionPlanId")]
        public SubscriptionPlan SubscriptionPlan { get; set; }

        [ForeignKey("PaymentRecordId")]
        public PaymentRecords PaymentRecord { get; set; }

        [ForeignKey("UserId")]
        public AspNetUsers AspNetUsers { get; set; }
    }
}
