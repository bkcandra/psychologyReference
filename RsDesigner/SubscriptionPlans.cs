//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RsDesigner
{
    using System;
    using System.Collections.Generic;
    
    public partial class SubscriptionPlans
    {
        public SubscriptionPlans()
        {
            this.UserSubscriptions = new HashSet<UserSubscriptions>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string RequiredRoles { get; set; }
        public int LengthValue { get; set; }
        public int LengthType { get; set; }
        public int CreatedUTC { get; set; }
        public string CreatedBy { get; set; }
        public int ModifiedUTC { get; set; }
        public string ModifiedBy { get; set; }
    
        public virtual ICollection<UserSubscriptions> UserSubscriptions { get; set; }
    }
}
