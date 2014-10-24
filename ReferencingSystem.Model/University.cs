using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferencingSystem.Model
{
    public partial class University
    {
        public University()
        {
            UniversityAdmin = new HashSet<UniversityAdmin>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string FullAddress { get; set; }

        // record information

        public int CreatedUTC { get; set; }

        public string CreatedBy { get; set; }

        public int ModifiedUTC { get; set; }

        public string ModifiedBy { get; set; }
        
        public ICollection<UniversityAdmin> UniversityAdmin { get; set; }
    }

    public partial class UniversityAdmin
    {

        [Key]
        public string UserId { get; set; }

        public int UniversityId { get; set; }

        public string ConfirmationToken { get; set; }

        public int ConfirmationTokenExpiry { get; set; }

        [ForeignKey("UniversityId")]
        public University University { get; set; }

        public UserProfiles UserProfiles { get; set; }
    }
}
