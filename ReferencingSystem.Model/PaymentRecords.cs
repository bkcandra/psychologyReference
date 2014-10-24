using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferencingSystem.Model
{
    public class PaymentRecords
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public int PlanId { get; set; }

        [Required]
        public string Token { get; set; }

        public string Status { get; set; }

        public string TransId { get; set; }

        public string PNRef { get; set; }
        
        public string PayerId { get; set; }

        public string Action { get; set; }

        public string ShippingAddress { get; set; }

        public string Amount { get; set; }
        
        public string TransFee{ get; set; }

        public string TaxAmount { get; set; }

        public bool Completed { get; set; }

        public string Message { get; set; }

        // record info
        public int CreatedUTC { get; set; }

        public string CreatedBy { get; set; }

        public int ModifiedUTC { get; set; }

        public string ModifiedBy { get; set; }

    }
}
