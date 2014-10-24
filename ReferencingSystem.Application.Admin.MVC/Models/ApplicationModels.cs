using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferencingSystem.Application.Admin.MVC.Models
{
    public class EmailSettingModels
    {
        public EmailSettingModels()
        {
            TemplateSettings = new List<EmailTemplateModels>();
        }
        public ICollection<EmailTemplateModels> TemplateSettings { get; set; }
    }

    public class EmailTemplateModels
    {
        public int Type { get; set; }
        public int EmailTemplateId { get; set; }
    }
}