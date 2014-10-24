using ReferencingSystem.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReferencingSystem.Application.Main.MVC.Models
{
    public class RefAnswerModel
    {
        public RefAnswerModel()
        {
            answer = new HashSet<ReferenceAnswer>();
        }
        public int ReferenceId { get; set; }
        public int RefFormId { get; set; }
        public string transId { get; set; }
        public ICollection<ReferenceAnswer> answer { get; set; }
    }

    public class JsonReferenceShareRecord
    {
        public int Id { get; set; }

        public int UniversityId { get; set; }

        public string UniversityName { get; set; }

        public string LinkToken { get; set; }

        public string CreatedDate { get; set; }
    }

    public class InitialCourse
    {
        public int CourseId;
        public int CourseLevelId;
        public string CourseText;

        public InitialCourse(int _courseId, int _courseLevelId, string _courseText)
        {
            CourseId = _courseId;
            CourseLevelId = _courseLevelId;
            CourseText = _courseText;
        }
    }
}