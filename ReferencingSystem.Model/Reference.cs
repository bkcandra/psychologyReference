using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferencingSystem.Model
{
    public partial class Reference
    {
        public Reference()
        {
            ReferenceAnswer = new HashSet<ReferenceAnswer>();
            ReferenceFiles = new HashSet<ReferenceFiles>();
            ReferenceShareRecord = new HashSet<ReferenceShareRecord>();
            ReferenceCourse = new HashSet<ReferenceCourse>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? FormId { get; set; }

        public string UserId { get; set; }

        [Required]
        public string TransId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [EmailAddress]
        public string RefEmail { get; set; }

        [StringLength(128)]
        public string RefUserId { get; set; }

        public string Note { get; set; }

        /// <summary>
        /// Whether or not this reference is active or inactive, status is changed 1                                                                        
        /// after some period
        /// </summary>
        public bool IsActive { get; set; }

        public int Status { get; set; }

        public int ExpiryDateUTC { get; set; }

        // record info

        public int CreatedUTC { get; set; }

        public string CreatedBy { get; set; }

        public int ModifiedUTC { get; set; }

        public string ModifiedBy { get; set; }

        public ICollection<ReferenceAnswer> ReferenceAnswer { get; set; }

        public ICollection<ReferenceFiles> ReferenceFiles { get; set; }

        public ICollection<ReferenceShareRecord> ReferenceShareRecord { get; set; }

        public ICollection<ReferenceCourse> ReferenceCourse { get; set; }

        [ForeignKey("FormId")]
        public RefForm RefForm { get; set; }

        [ForeignKey("UserId")]
        public AspNetUsers AspNetUsers { get; set; }
    }

    public partial class ReferenceAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ReferenceId { get; set; }

        public int QuestionId { get; set; }

        public string Answer { get; set; }

        [ForeignKey("ReferenceId")]
        public Reference Reference { get; set; }

    }

    public partial class ReferenceFiles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FileName { get; set; }

        public string Title { get; set; }

        public int ReferenceId { get; set; }

        public int FileSize { get; set; }

        public byte[] FileByte { get; set; }

        public int ModifiedUTC { get; set; }



        public string ModifiedBy { get; set; }

        public bool IsSaved { get; set; }

        [ForeignKey("ReferenceId")]
        public Reference Reference { get; set; }
    }

    public partial class ReferenceShareRecord
    {
        public ReferenceShareRecord()
        {
            DownloadRecords = new HashSet<DownloadRecords>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public int ReferenceId { get; set; }

        [Required]
        public string LinkToken { get; set; }

        //SchoolID  -- foreign to many
        public int UniversityId { get; set; }

        public int ClickCount { get; set; }

        public int CreatedUTC { get; set; }

        public string CreatedBy { get; set; }

        public int ModifiedUTC { get; set; }

        public string ModifiedBy { get; set; }

        [ForeignKey("ReferenceId")]
        public Reference Reference { get; set; }

        public ICollection<DownloadRecords> DownloadRecords { get; set; }
    }

    public partial class DownloadRecords
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int ReferenceShareRecordId { get; set; }

        public string IpAddress { get; set; }
        [Required]
        public string UserId { get; set; }

        public int CreatedUTC { get; set; }

        public string CreatedBy { get; set; }

        [ForeignKey("ReferenceShareRecordId")]
        public ReferenceShareRecord ReferenceShareRecord { get; set; }
    }

    public partial class ReferenceCourse
    {
        [Key]
        public int Id { get; set; }
        public int ReferenceId { get; set; }
        public int CourseId { get; set; }
        public int CourseLevelId { get; set; }
        public string CourseText { get; set; }
        [ForeignKey("ReferenceId")]
        public virtual Reference Reference { get; set; }
    }
}
