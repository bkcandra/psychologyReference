using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferencingSystem.Model
{
    public partial class RefForm
    {
        public RefForm()
        {
            Question = new HashSet<Question>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserType { get; set; }

        public int Type { get; set; }

        public string Name { get; set; }

        public string Readable { get; set; }

        public bool IsActive{ get; set; }

        public int Status { get; set; }

        public ICollection<Question> Question { get; set; }

        public int CreatedUTC { get; set; }

        public string CreatedBy { get; set; }

        public int ModifiedUTC { get; set; }

        public string ModifiedBy { get; set; }
    }

    public partial class Question
    {
        public Question()
        {
            QuestionOption = new HashSet<QuestionOption>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int FormId { get; set; }

        public int Type { get; set; }

        public int Order { get; set; }

        public int Group { get; set; }

        public string Text { get; set; }

        public int CreatedUTC { get; set; }

        public string CreatedBy { get; set; }

        public int ModifiedUTC { get; set; }

        public string ModifiedBy { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("FormId")]
        public RefForm RefForm { get; set; }

        public ICollection<QuestionOption> QuestionOption { get; set; }
    }

    public partial class QuestionOption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public int QuestionGroupId { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
    }
}
