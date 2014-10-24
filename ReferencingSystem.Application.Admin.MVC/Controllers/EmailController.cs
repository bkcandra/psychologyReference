using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReferencingSystem.Model;
using System.Threading.Tasks;
using ReferencingSystem.Utility;

namespace ReferencingSystem.Application.Admin.MVC.Controllers
{
    public class EmailController : Controller
    {
        private RsContext db = new RsContext();

        // GET: Email
        public async Task<ActionResult> Index(ReferencingSystem.Utility.SystemConstants.FormMessageId? message)
        {
            ViewBag.StatusMessage =
               message == ReferencingSystem.Utility.SystemConstants.FormMessageId.CreateSuccess ? SystemConstants.CreateSuccess
               : message == ReferencingSystem.Utility.SystemConstants.FormMessageId.DeleteSuccess ? SystemConstants.DeleteSuccess
               : message == ReferencingSystem.Utility.SystemConstants.FormMessageId.EditSuccess ? SystemConstants.EditSuccess
               : message == ReferencingSystem.Utility.SystemConstants.FormMessageId.Error ? SystemConstants.FormErrorMessage
               : message == ReferencingSystem.Utility.SystemConstants.FormMessageId.ErrorTemplateDelete ? SystemConstants.ErrorTemplateDelete
               : "";

            
            var model = await db.EmailSetting.ToListAsync();
            return View(model);
        }

        // POST: Email/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(List<EmailSetting> emailSetting)
        {
            ModelState.Clear();
            foreach (var item in emailSetting)
            {
                if (item.EmailTemplateId == 0)
                    ModelState.AddModelError("invalid template", "Please select a template for " + item.Name);
            }

            if (ModelState.IsValid)
            {
                ReferencingSystem.Utility.SystemConstants.FormMessageId? message;

                var Settings = await db.EmailSetting.ToListAsync();
                foreach (var item in emailSetting)
                {
                    var setting = Settings.Where(x => (x.EmailType == item.EmailType && x.Id == item.Id)).FirstOrDefault();
                    if (setting != null)
                        setting.EmailTemplateId = item.EmailTemplateId;
                }

                await db.SaveChangesAsync();
                message = SystemConstants.FormMessageId.EditSuccess;
                return RedirectToAction("Index", new { Message = message });
            }
            return View("index", emailSetting);
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class EmailTemplateController : Controller
    {
        private RsContext db = new RsContext();

        // GET: EmailTemplate
        public ActionResult Index()
        {
            return View(db.EmailTemplate.ToList());
        }

        // GET: EmailTemplate/Create
        public ActionResult Create()
        {
            return View(new EmailTemplate() { EmailIsHTML = true });
        }

        // POST: EmailTemplate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmailTemplate template)
        {
            if (ModelState.IsValid)
            {
                //set record information
                template.EmailIsHTML = true;
                template.ModifiedBy = template.CreatedBy = User.Identity.Name;
                template.ModifiedUTC = template.CreatedUTC = SystemConstants.SecondsSinceEpochUTC();

                db.EmailTemplate.Add(template);
                db.SaveChanges();
                return RedirectToAction("", "Email", null);
            }

            return View(template);
        }

        // GET: EmailTemplate/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailTemplate emailTemplate = db.EmailTemplate.Find(id);
            if (emailTemplate == null)
            {
                return HttpNotFound();
            }
            return View(emailTemplate);
        }

        // POST: EmailTemplate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmailTemplate emailTemplate)
        {
            if (ModelState.IsValid)
            {
                emailTemplate.EmailIsHTML = true;
                emailTemplate.ModifiedBy = User.Identity.Name;
                emailTemplate.ModifiedUTC = SystemConstants.SecondsSinceEpochUTC();

                db.Entry(emailTemplate).State = EntityState.Modified;
                db.SaveChanges();
                ReferencingSystem.Utility.SystemConstants.FormMessageId? message;
                message = SystemConstants.FormMessageId.EditSuccess;
                return RedirectToAction("", "Email", new { Message = message });
            }
            return View(emailTemplate);
        }

        // GET: EmailTemplate/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailTemplate emailTemplate = db.EmailTemplate.Find(id);
            if (emailTemplate == null)
            {
                return HttpNotFound();
            }
            return View(emailTemplate);
        }

        // POST: EmailTemplate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReferencingSystem.Utility.SystemConstants.FormMessageId? message;
            var settings = db.EmailSetting.ToList();
            EmailTemplate emailTemplate = db.EmailTemplate.Find(id);
            foreach (var item in settings)
            {
                if (item.EmailTemplateId == id)
                    ModelState.AddModelError("", "The template " + emailTemplate.EmailName + " is currently used on " + item.Name + ". Please change this setting to avoid empty email delivery.");
            }
            if (ModelState.IsValid)
            {
               
                db.EmailTemplate.Remove(emailTemplate);
                db.SaveChanges();
                message = SystemConstants.FormMessageId.DeleteSuccess;
                return RedirectToAction("", "Email", new { Message = message });
            }
            var model = db.EmailSetting.ToList();
            message = SystemConstants.FormMessageId.ErrorTemplateDelete;
            return RedirectToAction("index", "Email", new { Message = message });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

