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
    
    public partial class CourseLevels
    {
        public CourseLevels()
        {
            this.Courses = new HashSet<Courses>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreatedUTC { get; set; }
        public string CreatedBy { get; set; }
        public int ModifiedUTC { get; set; }
        public string ModifiedBy { get; set; }
    
        public virtual ICollection<Courses> Courses { get; set; }
    }
}
