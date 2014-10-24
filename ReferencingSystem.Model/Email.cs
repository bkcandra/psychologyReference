using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferencingSystem.Model
{
    public partial class EmailSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int EmailType { get; set; }

        public int EmailTemplateId { get; set; }

        public int ModifiedUTC { get; set; }

        public string ModifiedBy { get; set; }
    }

    public partial class EmailTemplate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name="Template Name")]
        public string EmailName { get; set; }

        [Required]
        [Display(Name = "Email Subject")]
        public string EmailSubject { get; set; }

        public string Emailcc { get; set; }

        [Required]
        [MaxLength]
        public string EmailBody { get; set; }

        public bool EmailIsHTML { get; set; }

        // record information

        public int CreatedUTC { get; set; }

        public string CreatedBy { get; set; }

        public int ModifiedUTC { get; set; }

        public string ModifiedBy { get; set; }

    }
}
