using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferencingSystem.Model
{
    public class UserProfiles
    {
        [Required]
        [Key]
        public string UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Gender { get; set; }

        [Display(Name = "Phone number")]
        public string Contact { get; set; }

        [Display(Name = "Other contact")]
        public string AltContact { get; set; }

        [Display(Name = "University Name")]
        public int? UniversityId { get; set; }

        [Display(Name = "Institution Name")]
        public string InstitutionName { get; set; }

        [Display(Name = "School")]
        public int? SchoolId { get; set; }

        [Display(Name = "School Name")]
        public string SchoolName { get; set; }

        [Display(Name = "Position")]
        public string Position { get; set; }

        [Display(Name = "Student Id / Staff Id")]
        public string InstitutionId { get; set; }

        public bool AgreeToTerms { get; set; }

        public int CreatedUTC { get; set; }

        public string CreatedBy { get; set; }

        public int ModifiedUTC { get; set; }

        public string ModifiedBy { get; set; }

        public int Status { get; set; }

        public int RefereeType { get; set; }

        [ForeignKey("UserId")]
        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual UniversityAdmin UniversityAdmin { get; set; }

    }
}
