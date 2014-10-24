using ReferencingSystem.Model;
using ReferencingSystem.Utility;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Data.Entity;
using System.Web.Security;
using System.Collections;
using System.Configuration;

namespace ReferencingSystem.Core.BF
{
    public class BusinessFunctionComponents
    {
        private RsContext db = new RsContext();

        #region Email
        /// <summary>
        /// Sending email to SendGrip SMTP. 
        /// </summary>
        /// <param name="from">'from' email aduser.UserProfilesess</param>
        /// <param name="to">'to' email aduser.UserProfilesess</param>
        /// <param name="subject">'subject' for this email</param>
        /// <param name="isHtml">is the message is Html</param>
        /// <param name="message">email body, parse your message with ParseEmail </param>
        public Task SendEmail(string from, string to, string subject, bool isHtml, string message, NetworkCredential credentials)
        {
            var myMessage = new SendGridMessage();
            myMessage.From = new MailAddress(from);

            myMessage.AddTo(to);

            myMessage.Subject = subject;
            if (isHtml)
                myMessage.Html = message;
            else
                myMessage.Text = message;

            var transportWeb = new Web(credentials);
            if (transportWeb != null)
            {
                return transportWeb.DeliverAsync(myMessage);
            }
            else
            {
                return Task.FromResult(0);
            }

        }

        public void ParseEmail(EmailTemplate emTemp, string userID, string token, int EmailTemplateType, string message = "")
        {
            var user = db.AspNetUsers.Where(x => x.Id.Equals(userID)).FirstOrDefault();
            var uProfiles = db.UserProfiles.Where(x => x.UserId.Equals(userID)).FirstOrDefault();

            if (uProfiles != null)
            {
                if (EmailTemplateType == (int)SystemConstants.EmailTemplateType.RefereeWelcomeEmail)
                {
                    //Provider Fullname
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@fullname]", uProfiles.FirstName + " " + uProfiles.LastName);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@loginurl]", SystemConstants.MainUrl + "/Account/login");
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@email]", user.Email);
                    //Provider ConfirmationTokenuri
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@confirmationTokenwithurl]", SystemConstants.MainUrl + "/Account/ConfirmEmail?" + SystemConstants.token + "=" + token + "&userId=" + userID);
                    //Provider ConfirmationToken
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@token]", token);
                    //Provider Confirmationurl
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@confirmationurl]", SystemConstants.MainUrl + "/Account/ConfirmEmail");
                    //Provider ConfirmationToken
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@cancelregistration]", SystemConstants.MainUrl + "/Account/CancelAccount?" + SystemConstants.userID + "=" + userID);
                }
                else if (EmailTemplateType == (int)SystemConstants.EmailTemplateType.UserWelcomeEmail)
                {
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@fullname]", uProfiles.FirstName + " " + uProfiles.LastName);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@loginurl]", SystemConstants.MainUrl + "/Account/login");
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@email]", user.Email);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@confirmationTokenwithurl]", SystemConstants.MainUrl + "/Account/ConfirmEmail?" + SystemConstants.token + "=" + token + "&userId=" + userID);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@token]", token);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@confirmationurl]", SystemConstants.MainUrl + "/Account/ConfirmEmail");
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@cancelregistration]", SystemConstants.MainUrl + "/Account/CancelAccount?" + SystemConstants.userID + "=" + userID);
                }
                else if (EmailTemplateType == (int)SystemConstants.EmailTemplateType.ForgotPassword)
                {
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@fullname]", uProfiles.FirstName + " " + uProfiles.LastName);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@email]", user.Email);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@recoverylinkwithtoken]", SystemConstants.MainUrl + "/Account/PasswordRecovery?" + SystemConstants.token + "=" + token);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@cancelregistration]", SystemConstants.MainUrl + "/Account/CancelAccount?" + SystemConstants.userID + "=" + userID);
                }
                else if (EmailTemplateType == (int)SystemConstants.EmailTemplateType.UserReferenceRequest)
                {
                    var r = db.Reference.Where(x => x.TransId == token).FirstOrDefault();
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@fullname]", uProfiles.FirstName + " " + uProfiles.LastName);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@refemail]", r.RefEmail);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@refereeregistrationtoken]", r.Token);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@refereeregistrationtokenwithurl]", SystemConstants.MainUrl + "/Referee/Register?" + SystemConstants.authToken + "=" + r.Token);
                }
                else if (EmailTemplateType == (int)SystemConstants.EmailTemplateType.ReferenceStatusChanged)
                {
                    var r = db.Reference.Where(x => x.TransId == token).FirstOrDefault();

                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@fullname]", uProfiles.FirstName + " " + uProfiles.LastName);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@referencetrans]", r.TransId);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@referencestatus]", GetRefStatus(r.Status));
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@message]", message);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@message]", message);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@url]", SystemConstants.MainUrl);
                }
            }
        }

        #endregion

        #region Subscription

        public bool CreateUserSubscription(String UserId, int PlanId, int PaymentRecordId, out string message, out int expiry)
        {
            expiry = 0;
            var PaymentRecord = db.PaymentRecords.Where(x => x.Id == PaymentRecordId).FirstOrDefault();
            if (PaymentRecord == null)
            {
                message = "Invalid payment records, cannot read payment confirmation from payment provider";
                return false;
            }

            var plan = db.SubscriptionPlan.Where(x => x.Id == PlanId).FirstOrDefault();
            if (plan == null)
            {
                message = "Cannot find the payment plan for transaction #" + PaymentRecord.TransId;

                return false;
            }

            var userSub = db.UserSubscription.Create();
            userSub.UserId = UserId;
            userSub.subscriptionPlanId = PlanId;
            userSub.PaymentRecordId = PaymentRecordId;

            var now = SystemConstants.SecondsSinceEpochUTC();
            if (plan.LengthType == (int)SystemConstants.DatetimeLengthType.Day)
                expiry = userSub.ExpiryUTC = SystemConstants.SecondsSinceEpochUTC(DateTime.Now.AddDays(plan.LengthValue));
            else if (plan.LengthType == (int)SystemConstants.DatetimeLengthType.Month)
                expiry = userSub.ExpiryUTC = SystemConstants.SecondsSinceEpochUTC(DateTime.Now.AddMonths(plan.LengthValue));
            else if (plan.LengthType == (int)SystemConstants.DatetimeLengthType.Year)
                expiry = userSub.ExpiryUTC = SystemConstants.SecondsSinceEpochUTC(DateTime.Now.AddYears(plan.LengthValue));

            userSub.ModifiedBy = userSub.CreatedBy = "System";
            userSub.ModifiedUTC = userSub.CreatedUTC = SystemConstants.SecondsSinceEpochUTC();

            db.UserSubscription.Add(userSub);
            db.SaveChangesAsync();
            message = "Success creating user subscription";
            return true;
        }

        public bool UserSubscriptionRequired(string roles)
        {
            var subscriptionPlans = db.SubscriptionPlan.Where(x => x.IsActive == true).Select(x => x.RequiredRoles).ToList();

            if (subscriptionPlans.Contains(roles))
                return true;

            return false;
        }

        public bool UserSubscriptionRequired(IEnumerable<string> roles)
        {
            var subscriptionPlans = db.SubscriptionPlan.Where(x => x.IsActive == true).Select(x => x.RequiredRoles).ToList();

            foreach (var role in roles)
                if (subscriptionPlans.Contains(role))
                    return true;
            return false;
        }

        #endregion

        #region Reference

        public bool ValidateReferenceToken(string token, out int refId)
        {
            refId = 0;
            var Ref = db.Reference.Where(x => x.Token.Equals(token, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (Ref == null)
                return false;
            refId = Ref.Id;
            return true;

        }

        public bool ValidateAuthReferenceToken(string token)
        {
            var reference = db.Reference.Where(x => (x.RefUserId == Guid.Empty.ToString() && x.Token == token)).FirstOrDefault();
            if (reference != null)
                return true;
            else return false;
        }

        public bool ValidateAuthReferenceToken(string token, string email)
        {
            var reference = db.Reference.Where(x => (x.RefUserId == Guid.Empty.ToString() && x.Token == token && x.RefEmail == email)).FirstOrDefault();
            if (reference != null)
                return true;
            else return false;
        }

        public bool CreateReference(InitialReference model, out string message, out string transId)
        {
            var Reference = db.Reference.Create();

            Reference.TransId = SystemConstants.GenerateRandomString(6, new Random());
            Reference.Token = SystemConstants.GenerateUniqueString(32);
            Reference.RefEmail = model.RefereeEmail;
            Reference.RefUserId = Guid.Empty.ToString();
            Reference.UserId = model.UserId;

            if (string.IsNullOrEmpty(model.Note))
                model.Note = string.Empty;
            Reference.Note = model.Note;
            Reference.IsActive = true;
            Reference.Status = (int)SystemConstants.Status.WaitingForReferee;
            Reference.ExpiryDateUTC = SystemConstants.SecondsSinceEpochUTC(DateTime.UtcNow.AddMonths(6));
            // record info
            Reference.CreatedUTC = Reference.ModifiedUTC = SystemConstants.SecondsSinceEpochUTC();
            Reference.CreatedBy = Reference.ModifiedBy = model.UserId;

            db.Reference.Add(Reference);


            foreach (var item in model.ReferenceCourse)
            {
                Reference.ReferenceCourse.Add(item);
            }

            db.SaveChanges();
            transId = Reference.TransId;

            message = "an authentication token has been given to email address " + Reference.RefEmail + ". You can register using the following link <br/>" +
                        SystemConstants.MainUrl + "/Referee/Register?" + SystemConstants.authToken + "=" + Reference.Token +
                      "or, if that doesn't work use this Reference code to register at " + SystemConstants.MainUrl + "/Referee/Register" + "<br/> "
                      + Reference.Token;

            return true;
        }

        public void ValidateReference(Reference reference, string userId)
        {
            var dbRef = db.Reference.Where(x => (x.Id == reference.Id && x.TransId == reference.TransId)).FirstOrDefault();
            var dbuserProfile = db.UserProfiles.Where(x => (x.UserId == userId)).FirstOrDefault();
            var refForm = new ReferencingSystem.Model.RefForm();
            if (dbuserProfile.RefereeType == (int)ReferencingSystem.Utility.SystemConstants.RefereeType.Academic)
            {
                refForm = db.RefForm.Where(x => (x.UserType == (int)ReferencingSystem.Utility.SystemConstants.RefereeType.Academic && x.IsActive)).FirstOrDefault();
            }
            else
            {
                refForm = db.RefForm.Where(x => (x.UserType == (int)ReferencingSystem.Utility.SystemConstants.RefereeType.Professional && x.IsActive)).FirstOrDefault();
            }

            if (dbRef != null && dbuserProfile != null)
            {
                dbRef.FormId = refForm.Id;
                dbRef.RefUserId = reference.RefUserId = userId;
                dbRef.Status = reference.Status = (int)SystemConstants.Status.PendingSubmission;
                db.SaveChanges();
            }
        }

        public bool SubmitRefereeReference(Reference Ref, string user, out string message)
        {
            message = "Could not find transaction.";
            var dbRef = db.Reference.Where(x => (x.TransId == Ref.TransId && x.Id == Ref.Id)).FirstOrDefault();
            if (Ref == null || dbRef == null)
                return false;

            dbRef.Status = (int)SystemConstants.Status.ReturnedToUser;
            dbRef.ModifiedUTC = SystemConstants.SecondsSinceEpochUTC();
            dbRef.ModifiedBy = user;

            db.SaveChanges();
            message = "Success submitting reference #" + Ref.TransId + ".";
            return true;
        }

        private string GetRefStatus(int status)
        {
            string message = "";
            if (status == (int)ReferencingSystem.Utility.SystemConstants.Status.CreatedByUser)
            {
                message = SystemConstants.CreatedByUser;
            }
            else if (status == (int)ReferencingSystem.Utility.SystemConstants.Status.WaitingForReferee)
            {
                message = SystemConstants.WaitingForReferee;
            }
            else if (status == (int)ReferencingSystem.Utility.SystemConstants.Status.PendingSubmission)
            {
                message = SystemConstants.PendingSubmission;
            }
            else if (status == (int)ReferencingSystem.Utility.SystemConstants.Status.ReturnedToUser)
            {
                message = SystemConstants.ReturnedToUser;
            }
            else if (status == (int)ReferencingSystem.Utility.SystemConstants.Status.DeniedByReferee)
            {
                message = SystemConstants.DeniedByReferee;
            }
            else if (status == (int)ReferencingSystem.Utility.SystemConstants.Status.SharedAndCompleted)
            {
                message = SystemConstants.SharedAndCompleted;
            }
            return message;
        }

        public void RemoveUnsavedFiles(string transId, bool removeSaved)
        {
            Reference reference = db.Reference.Where(x => x.TransId == transId).FirstOrDefault();
            if (reference != null)
            {
                var fileList = db.ReferenceFiles.Where(x => (x.ReferenceId == reference.Id && !x.IsSaved));
                db.ReferenceFiles.RemoveRange(fileList);
                db.SaveChanges();
            }
        }



        public bool DenyReference(string transId, string user, out string message)
        {
            message = "Could not find transaction.";
            var dbRef = db.Reference.Where(x => (x.TransId == transId)).FirstOrDefault();
            if (dbRef == null || dbRef == null)
                return false;

            dbRef.Status = (int)SystemConstants.Status.DeniedByReferee;
            dbRef.ModifiedUTC = SystemConstants.SecondsSinceEpochUTC();
            dbRef.ModifiedBy = user;

            db.SaveChanges();
            message = "Success deny reference request #" + transId + ".";
            return true;
        }

        public bool ShareReference(InitialShareReference ShareRef, out string message)
        {
            if (!string.IsNullOrEmpty(ShareRef.ShareTransId))
            {
                var dbRef = db.Reference.Where(x => (x.TransId == ShareRef.ShareTransId)).Include(u => u.ReferenceShareRecord).FirstOrDefault();
                if (dbRef != null)
                {
                    if (dbRef.Status == (int)SystemConstants.Status.DeniedByReferee)
                    {
                        message = "Cannot share reference, application is denied by referee.";
                        return false;
                    }
                    var shareRecords = dbRef.ReferenceShareRecord.Select(x => x.UniversityId).ToList();
                    foreach (var item in ShareRef.UniversityId)
                    {
                        if (!shareRecords.Contains(item))
                        {
                            if (dbRef.Status != (int)SystemConstants.Status.SharedAndCompleted)
                                dbRef.Status = (int)SystemConstants.Status.SharedAndCompleted;

                            var newRefShare = db.ReferenceShareRecord.Create();

                            newRefShare.LinkToken = SystemConstants.GenerateUniqueString(16);
                            newRefShare.ClickCount = 0;
                            newRefShare.CreatedUTC = newRefShare.ModifiedUTC = SystemConstants.SecondsSinceEpochUTC();
                            newRefShare.CreatedBy = newRefShare.ModifiedBy = ShareRef.User;
                            newRefShare.UniversityId = item;

                            dbRef.ReferenceShareRecord.Add(newRefShare);
                        }
                    }

                    db.SaveChanges();
                    message = "Reference has is shared";
                    return true;
                }
                message = "Invalid transId, could not find reference";
                return false;
            }
            message = "Invalid transId, transId Is null or empty";
            return false;
        }



        public bool UpdateRefMessage(ref string message, string transId, string role)
        {
            var r = db.Reference.Where(x => x.TransId == transId).FirstOrDefault();
            if (r == null)
            {
                message = "Invalid transId";
                return false;
            }
            StringBuilder sb = new StringBuilder();
            
            sb.Append(DateTime.Now.ToString("dd/MMM/yyyy") + " - " +role + " :"+ System.Environment.NewLine + message + System.Environment.NewLine + System.Environment.NewLine + r.Note);
            message = r.Note = sb.ToString();
            db.SaveChanges();
            return true;
        }
    }
        #endregion

    public class InitialReference
    {
        public InitialReference()
        {
            ReferenceCourse = new List<ReferenceCourse>();
        }
        [Required]
        public string UserId { get; set; }
        [Required]
        [EmailAddress]
        public string RefereeEmail { get; set; }
        [Required]
        public List<ReferenceCourse> ReferenceCourse { get; set; }


        public String Note { get; set; }
    }

    public class InitialShareReference
    {
        public InitialShareReference()
        {
            UniversityId = new List<int>();
        }
        public string ShareTransId { get; set; }
        public string User { get; set; }
        public List<int> UniversityId { get; set; }
    }

}
