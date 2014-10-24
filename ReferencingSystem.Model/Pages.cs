using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferencingSystem.Model
{
    public partial class Pages
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        public int TemplateId { get; set; }
        [Required]
        [StringLength(500)]
        public string MetaTag { get; set; }

        [Required]
        [StringLength(1000)]
        public string MetaDescription { get; set; }

        public int CreatedUTC { get; set; }

        public int ModifiedUTC { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [MaxLength]
        public string Content { get; set; }

        public bool Published { get; set; }


    }
}
