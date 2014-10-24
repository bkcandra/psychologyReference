using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferencingSystem.Model
{
    public partial class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public int CourseLevelId { get; set; }

        // record info
        public int CreatedUTC { get; set; }

        public string CreatedBy { get; set; }

        public int ModifiedUTC { get; set; }

        public string ModifiedBy { get; set; }

       
    }

    public partial class CourseLevel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // record info
        public int CreatedUTC { get; set; }

        public string CreatedBy { get; set; }

        public int ModifiedUTC { get; set; }

        public string ModifiedBy { get; set; }

    }
}
