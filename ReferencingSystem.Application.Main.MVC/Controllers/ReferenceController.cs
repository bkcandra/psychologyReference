using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReferencingSystem.Model;
using ReferencingSystem.Application.Main.MVC.Models;
using ReferencingSystem.Utility;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using ReferencingSystem.Core.BF;
using System.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Ionic.Zip;
using System.IO.Pipes;
using System.Threading;
using RazorPDF;
using System.Text;
using System.Web.UI;

namespace ReferencingSystem.Application.Main.MVC.Controllers
{
    public class ViewerController : Controller
    {
        private RsContext db = new RsContext();

        public ActionResult PdfViewer(string Id)
        {
            int refId = 0;
            if (string.IsNullOrEmpty(Id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (!new BusinessFunctionComponents().ValidateReferenceToken(Id, out refId))
                return HttpNotFound();

            return new Rotativa.ActionAsPdf("Reference", new { @Id = refId });
        }

        public async Task<ActionResult> Reference(int Id)
        {
            if (Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference reference = await db.Reference
                .Include(x => x.ReferenceCourse)
                .Include(x => x.ReferenceAnswer)
                .Include(x => x.ReferenceFiles)
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();

            if (reference == null)
            {
                return HttpNotFound();
            }   
            
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", reference.UserId);
            return View(reference);
        }

        public ActionResult PdfDownload(string Id)
        {
            int refId = 0;
            if (string.IsNullOrEmpty(Id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (!new BusinessFunctionComponents().ValidateReferenceToken(Id, out refId))
                return HttpNotFound();

            Reference reference = db.Reference
               .Include(x => x.ReferenceCourse)
               .Include(x => x.ReferenceAnswer)
               .Include(x => x.ReferenceFiles)
               .Where(x => x.Id == refId)
               .FirstOrDefault();

            ViewEngineResult result = ViewEngines.Engines.FindView(this.ControllerContext, "reference", "_Layout");
            string htmlTextView = GetViewToString(this.ControllerContext, result, reference);

            byte[] toBytes = Encoding.Unicode.GetBytes(htmlTextView);

            return File(toBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "template.html");
        }

        private string GetViewToString(ControllerContext context, ViewEngineResult result, object model)
        {
            string viewResult = "";
            var viewData = ViewData;
            viewData.Model = model;
            TempDataDictionary tempData = new TempDataDictionary();
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter output = new HtmlTextWriter(sw))
                {
                    ViewContext viewContext = new ViewContext(context,
                        result.View, viewData, tempData, output);
                    result.View.Render(viewContext, output);
                }
                viewResult = sb.ToString();
            }
            return viewResult;
        }

    }

    [Authorize(Roles = SystemConstants.SchoolAdminRole + "," + SystemConstants.RefereeRole)]
    public class ReferenceController : Controller
    {
        private RsContext db = new RsContext();
        private BusinessFunctionComponents bfc = new BusinessFunctionComponents();

        public class HttpParamActionAttribute : ActionNameSelectorAttribute
        {
            public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
            {
                if (actionName.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                    return true;

                var request = controllerContext.RequestContext.HttpContext.Request;
                return request[methodInfo.Name] != null;
            }
        }

        // GET: Reference/Details/5

        public async Task<ActionResult> Details(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference reference = await db.Reference
                .Where(x => x.TransId == Id)
                .Include(x => x.ReferenceCourse)
                .Include(x => x.ReferenceAnswer)
                .Include(x => x.ReferenceFiles)
                .FirstOrDefaultAsync();
 
            if (reference == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", reference.UserId);
            return View(reference);
        }


        public ActionResult Cancel(string Id)
        {
            bfc.RemoveUnsavedFiles(Id, false);

            return RedirectToAction("Index", "References", null);
        }

        [HttpGet]
        public async Task<ActionResult> DeleteFile(string TransId, int Id)
        {
            var refFiles = db.Reference.Where(x => x.TransId == TransId).Select(x => new
            {
                Reference = x,
                file = x.ReferenceFiles.Where(f => f.Id == Id).FirstOrDefault(),
            })
             .ToList();
            if (refFiles == null) return HttpNotFound();
            if (refFiles.FirstOrDefault().file == null)
                return HttpNotFound();

            db.ReferenceFiles.Remove(refFiles.FirstOrDefault().file);
            await db.SaveChangesAsync();

            return RedirectToAction("Edit", "Reference", new { Id = TransId });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteFile(string TransId, string fName, int fSize)
        {

            var refFiles = db.Reference.Where(x => x.TransId == TransId).Select(x => new
            {
                Reference = x,
                file = x.ReferenceFiles.Where(f => (f.FileName == fName && f.FileSize == fSize)).FirstOrDefault(),
            })
             .ToList();
            if (refFiles == null) return HttpNotFound();
            if (refFiles.FirstOrDefault().file == null)
                return HttpNotFound();

            db.ReferenceFiles.Remove(refFiles.FirstOrDefault().file);
            await db.SaveChangesAsync();

            return RedirectToAction("Edit", "Reference", new { Id = TransId });
        }

        [HttpGet]
        public ActionResult File(string TransId, int Id)
        {
            var refFiles = db.Reference.Where(x => x.TransId == TransId)
                .Include(x => x.ReferenceFiles)
                .FirstOrDefault();
            if (refFiles == null) return HttpNotFound();
            var file = refFiles.ReferenceFiles.Where(x => x.Id == Id).FirstOrDefault();
            if (file == null)
                return HttpNotFound();

            byte[] contents = file.FileByte;
            return File(contents, System.Net.Mime.MediaTypeNames.Application.Octet, file.FileName);
        }

        [HttpGet]
        public ActionResult Files(string TransId, bool includeMain = true, bool asArchive = true)
        {
            var reference = db.Reference.Where(x => x.TransId == TransId)
               .Include(x => x.ReferenceFiles)
               .FirstOrDefault();
            if (reference == null) return HttpNotFound();
            var files = reference.ReferenceFiles.ToList();
            if (!files.Any())
            {
                using (WebClient Client = new WebClient())
                {
                    var url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~/Viewer/PdfViewer/" + reference.Token));
                    return File(Client.DownloadData(url), System.Net.Mime.MediaTypeNames.Application.Octet, reference.TransId + ".pdf");
                }
            }
            var readable =
                GetPipedStream(output =>
                {
                    using (var zip = new ZipFile())
                    {
                        if (includeMain)
                        {
                            using (WebClient Client = new WebClient())
                            {
                                var url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~/Viewer/PdfViewer/" + reference.Token));
                                zip.AddEntry(reference.TransId + ".pdf", Client.DownloadData(url));
                            }
                        }
                        foreach (var file in files)
                            zip.AddEntry(file.FileName, file.FileByte);

                        zip.Save(output);
                    }
                });

            var fileResult = new FileStreamResult(readable, System.Net.Mime.MediaTypeNames.Application.Zip);
            fileResult.FileDownloadName = "MultiplePDFs.zip";

            return fileResult;
        }

        static Stream GetPipedStream(Action<Stream> writeAction)
        {
            AnonymousPipeServerStream pipeServer = new AnonymousPipeServerStream();
            ThreadPool.QueueUserWorkItem(s =>
            {
                using (pipeServer)
                {
                    writeAction(pipeServer);
                    pipeServer.WaitForPipeDrain();
                }
            });
            return new AnonymousPipeClientStream(pipeServer.GetClientHandleAsString());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DenyRequest(string Id)
        {
            string message = "";
            bfc.RemoveUnsavedFiles(Id, true);
            var Ref = await db.Reference.Where(x => x.TransId == Id).FirstOrDefaultAsync();
            if (bfc.DenyReference(Id, User.Identity.Name, out message))
            {
                // sending notification email to student.
                var userP = db.AspNetUsers.Where(x => x.Id == Ref.UserId).FirstOrDefault();
                var templateID = db.EmailSetting.Where(x => x.EmailType == (int)SystemConstants.EmailTemplateType.ReferenceStatusChanged).FirstOrDefault().EmailTemplateId; ;
                var template = db.EmailTemplate.Where(x => x.Id == templateID).FirstOrDefault();
                var netCred = new NetworkCredential(
                     ConfigurationManager.AppSettings[SystemConstants.SendGridUsernameAppSetting],
                     ConfigurationManager.AppSettings[SystemConstants.SendGridPassAppSetting]
                     );
                var emailBody = "";
                var emailSubject = "Reference Status Changed";
                if (template == null)
                {
                    emailBody = "Dear " + userP.UserName + "<br/>The reference status for your reference #" + Id + " has changed. please check your account.";
                }
                else
                {
                    template.EmailBody = HttpUtility.HtmlDecode(template.EmailBody);
                    new BusinessFunctionComponents().ParseEmail(template, userP.Id, Id, (int)SystemConstants.EmailTemplateType.ReferenceStatusChanged);
                    emailSubject = template.EmailSubject;
                    emailBody = template.EmailBody;
                }
                await bfc.SendEmail(SystemConstants.fromEmailAddress, userP.Email, emailSubject, true, emailBody, netCred);
                return RedirectToAction("", "References", new { status = SystemConstants.ReferenceMessage.RefereeSuccessSubmit });
            }

            return RedirectToAction("Index", "References", new { message = "Cancel code not configured" });
        }

        // GET: Reference/Edit/5
        public async Task<ActionResult> Edit(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference reference = await db.Reference.Where(x => x.TransId == Id).Include(x => x.ReferenceCourse).Include(x => x.ReferenceAnswer).Include(x=>x.ReferenceFiles).FirstOrDefaultAsync();
            if (reference == null)
            {
                return HttpNotFound();
            }
            if (reference.RefUserId == Guid.Empty.ToString())
                new BusinessFunctionComponents().ValidateReference(reference, User.Identity.GetUserId());
            if (reference.Status >= (int)SystemConstants.Status.ReturnedToUser)
                return View("Details", reference);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", reference.UserId);
            return View(reference);
        }

        // POST: Reference/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,UserId,TransId,Token,RefEmail,RefUserId,CourseId,Note,IsActive,Status,ExpiryDateUTC,CreatedUTC,CreatedBy,ModifiedUTC,ModifiedBy")] Reference reference)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reference).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", reference.UserId);
            return View(reference);
        }

        [HttpParamAction]

        public async Task<ActionResult> SaveRef(RefAnswerModel model)
        {
            await SaveAnswer(model);
            return RedirectToAction("", "References", null);
        }

        [HttpParamAction]
        public async Task<ActionResult> PreviewRef(RefAnswerModel model)
        {
            await SaveAnswer(model);

            //validate Answer
            var FormQuestions = db.Question.Where(x => x.FormId == model.RefFormId).OrderBy(x => x.Order).ToList();
            foreach (var item in model.answer)
            {
                var q = FormQuestions.Where(x => x.Id == item.QuestionId).Select(x => x.Text).FirstOrDefault();
                if (string.IsNullOrEmpty(item.Answer))
                    ModelState.AddModelError("", "Answer for question '" + q + "' is required.");
            }
            Reference reference = await db.Reference.Where(x => x.TransId == model.transId).FirstOrDefaultAsync();
            if (!ModelState.IsValid)
            {
                return View("Edit", reference);
            }
            else
            {
                if (reference == null)
                {
                    return HttpNotFound();
                }
                return View("Details", reference);
            }
        }

        public ActionResult UploadFiles(ICollection<HttpPostedFileBase> files, int RefId)
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            List<ReferenceFiles> rf = new List<ReferenceFiles>();
            try
            {

                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        var rFile = db.ReferenceFiles.Create();
                        rFile.FileName = rFile.Title = Path.GetFileName(file.FileName);
                        rFile.ModifiedBy = User.Identity.Name;
                        rFile.ModifiedUTC = SystemConstants.SecondsSinceEpochUTC();
                        rFile.ReferenceId = RefId;
                        rFile.IsSaved = false;
                        rFile.FileSize = file.ContentLength;

                        byte[] uploadedFile = new byte[file.InputStream.Length];
                        file.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

                        rFile.FileByte = uploadedFile;
                        db.ReferenceFiles.Add(rFile);
                        rf.Add(rFile);

                    }
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }


            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }

        public async Task<ActionResult> SubmitRef(Reference reference)
        {
            string message = "";

            if (bfc.SubmitRefereeReference(reference, User.Identity.Name, out message))
            {
                // sending notification email to student.
                var userP = db.AspNetUsers.Where(x => x.Id == reference.UserId).FirstOrDefault();
                var templateID = db.EmailSetting.Where(x => x.EmailType == (int)SystemConstants.EmailTemplateType.ReferenceStatusChanged).FirstOrDefault().EmailTemplateId; ;
                var template = db.EmailTemplate.Where(x => x.Id == templateID).FirstOrDefault();
                var netCred = new NetworkCredential(
                     ConfigurationManager.AppSettings[SystemConstants.SendGridUsernameAppSetting],
                     ConfigurationManager.AppSettings[SystemConstants.SendGridPassAppSetting]
                     );
                var emailBody = "";
                var emailSubject = "Reference Status Changed";
                if (template == null)
                    emailBody = "Dear " + userP.UserName + "<br/>The reference status for your reference #" + reference.TransId + " has changed. please check your account.";
                else
                {
                    template.EmailBody = HttpUtility.HtmlDecode(template.EmailBody);
                    new BusinessFunctionComponents().ParseEmail(template, reference.UserId, reference.TransId, (int)SystemConstants.EmailTemplateType.ReferenceStatusChanged);
                    emailBody = template.EmailBody;
                    emailSubject = template.EmailSubject;
                }
                await bfc.SendEmail(SystemConstants.fromEmailAddress, userP.Email, emailSubject, true, emailBody, netCred);
                return RedirectToAction("", "References", new { status = SystemConstants.ReferenceMessage.RefereeSuccessSubmit });
            }
            return RedirectToAction("", "References", new { status = SystemConstants.ReferenceMessage.Error, message = message });
        }

        public async Task<int> SaveAnswer(RefAnswerModel model)
        {
            var Records = db.ReferenceAnswer.Where(x => x.ReferenceId == model.ReferenceId).ToList();
            db.ReferenceAnswer.RemoveRange(Records);
            db.ReferenceAnswer.AddRange(model.answer);
            return await db.SaveChangesAsync();
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
