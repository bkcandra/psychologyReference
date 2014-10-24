using ReferencingSystem.Application.Main.MVC.Models;
using ReferencingSystem.Core.BF;
using ReferencingSystem.Model;
using ReferencingSystem.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ReferencingSystem.Application.Main.MVC.Controllers
{
    [Authorize(Roles = SystemConstants.UserRole + "," + SystemConstants.RefereeRole + "," + SystemConstants.SchoolAdminRole)]
    public class ReferencesController : Controller
    {
        private BusinessFunctionComponents bfc = new BusinessFunctionComponents();

        // GET: Application
        public ActionResult Index()
        {
            if (User.IsInRole(SystemConstants.UserRole))
                return View();
            else if (User.IsInRole(SystemConstants.RefereeRole))
                return View("Referee");
            else
                return View("SchoolAdmin");
        }
        [HttpPost]
        public async Task<ActionResult> Index(InitialReference model)
        {
            string message = "";
            string transId = "";
            if (!model.ReferenceCourse.Any())
                ModelState.AddModelError("", "Invalid course, Please specify course for this reference");
            if (ModelState.IsValid)
            {
                var results = bfc.CreateReference(model, out message, out transId);
                if (results)
                {
                    var db = new RsContext();
                    // sending notification email to teacher.
                    var templateID = db.EmailSetting.Where(x => x.EmailType == (int)SystemConstants.EmailTemplateType.UserReferenceRequest).FirstOrDefault().EmailTemplateId; ;
                    var template = db.EmailTemplate.Where(x => x.Id == templateID).FirstOrDefault();
                    var netCred = new NetworkCredential(
                         ConfigurationManager.AppSettings[SystemConstants.SendGridUsernameAppSetting],
                         ConfigurationManager.AppSettings[SystemConstants.SendGridPassAppSetting]
                         );
                    var emailBody = "";

                    if (template == null)
                        emailBody = "==DEMO==<br/>Dear " + model.RefereeEmail + "<br/>A student has requested a reference to this email address. Your email, " + model.RefereeEmail + " has been registered with us.  please see the following message to continue<br/>" + message;
                    else
                    {
                        template.EmailBody = HttpUtility.HtmlDecode(template.EmailBody);
                        bfc.ParseEmail(template, model.UserId, transId, (int)SystemConstants.EmailTemplateType.UserReferenceRequest);
                        emailBody = template.EmailBody;
                    }
                    await bfc.SendEmail(SystemConstants.fromEmailAddress, model.RefereeEmail, template.EmailSubject, true, emailBody, netCred);


                    return RedirectToAction("");
                }
                else
                {

                }
            }
            return View();
        }

        public dynamic AddCourse(int courseId, int courseLevelId, string CourseText = "")
        {
            if (courseId == 0 && string.IsNullOrEmpty(CourseText))
            {
                return Json(new { status = false, Message = "Invalid area of study name" });
            }
            InitialCourse course = new InitialCourse(courseId, courseLevelId, CourseText);
            return PartialView("_AddSelectCoursePartial", course);
        }

        [HttpPost]
        public ActionResult ShareCourseJson(InitialShareReference Initial)
        {
            if (string.IsNullOrEmpty(Initial.ShareTransId) && string.IsNullOrEmpty(Initial.User))
                return Json(new { Status = false, Message = "Invalid share request" });
            else if (!Initial.UniversityId.Any())
                return Json(new { Status = false, Message = "Empty universityId" });

            string message = "";

            if (new BusinessFunctionComponents().ShareReference(Initial, out message))
            {
                return Json(new { Status = true, Message = message });
            }
            else
                return Json(new { Status = false, Message = "Cannot share records" });
        }
        
        [HttpPost]
        public dynamic SaveNotes(string message, string transId, string userId)
        {
            if (!string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(transId))
            {
                if (bfc.UpdateRefMessage(ref message, transId, SystemConstants.RefereeRole))
                    return Json(new { Status = true, Message = "Success updating note", Note = message });
                return Json(new { Status = false, Message = message });
            }
            else
                return Json(new { Status = false, Message = "Bad Request, invalid message or ReferenceId" });
        }
    }
}