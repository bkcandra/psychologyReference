using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;



namespace ReferencingSystem.Utility
{
    public class SystemConstants
    {
        public static string token = "code";
        public static string authToken = "authToken";
        public static string userID = "uid";
        public const string UserRole = "User";
        public const string RefereeRole = "Referee";
        public const string SchoolAdminRole = "SchoolAdmin";
        public const string SystemAdminRole = "Administrator";
        public static string MainUrl = "refsystem.azurewebsites.net";
        public static string AdministrationUrl = "adminrefsystem.cloudapp.net";

        public static string SendGridUsernameAppSetting = "SendGridUsername";
        public static string SendGridPassAppSetting = "SendGridPass";
        public static string fromEmailAddress = "noreply@refsystem.azurewebsites.net";

        #region Application Message

        public static string EditSuccess = "Your changes has been saved.";
        public static string CreateSuccess = "New record has been saved.";
        public static string DeleteSuccess = "Record has been deleted.";
        public static string FormErrorMessage = "An error has occurred.";
        public static string ErrorTemplateDelete = "Template is currently in used. Please change the setting to avoid empty email delivery.";
        #endregion

        #region Reference status

        public static string CreatedByUser = "Created by user";
        public static string WaitingForReferee = "Awaiting referee response";
        public static string PendingSubmission = "Waiting for referee submission";
        public static string ReturnedToUser = "Returned to user";
        public static string SharedAndCompleted = "Shared ";
        public static string DeniedByReferee = "Denied by reference ";

        public enum Status { CreatedByUser = 1, WaitingForReferee = 2, PendingSubmission = 3, ReturnedToUser = 4, DeniedByReferee = 5, SharedAndCompleted = 6 }
        #endregion

        #region Referee
        public enum RefereeType { Academic = 1, Professional = 2 }
        public static List<SelectListItem> RetrieveRefereeType()
        {
            var RefereeList = new List<SelectListItem>();
            RefereeList.Add(new SelectListItem() { Selected = true, Text = "-- Select One --", Value = "0" });
            RefereeList.Add(new SelectListItem() { Selected = false, Text = SystemConstants.RefereeType.Academic.ToString(), Value = ((int)SystemConstants.RefereeType.Academic).ToString() });
            RefereeList.Add(new SelectListItem() { Selected = false, Text = SystemConstants.RefereeType.Professional.ToString(), Value = ((int)SystemConstants.RefereeType.Professional).ToString() });
            return RefereeList;
        }
        #endregion

        public enum QuestionType { TextQuestion = 1, MultipleChoiceSelectOne = 2, MultipleChoiceSelectMultiple = 3, SelectCourseAdmission = 4 }
        public enum LengthType { days = 1, month = 2, year = 3 }
        public enum Gender { Male = 1, Female = 2, NotSpecified = 3 }
        public enum EmailTemplateType { UserWelcomeEmail = 1, RefereeWelcomeEmail = 2, ForgotPassword = 3, UserReferenceRequest = 4, ReferenceStatusChanged = 5 }
        public enum DatetimeLengthType { Day = 1, Month = 2, Year = 3 }

        public enum FormMessageId { CreateSuccess, EditSuccess, DeleteSuccess, Error, ErrorTemplateDelete }
        public enum ReferenceMessage { CreateSuccess, EditSuccess, UserSuccessSubmit, RefereeSuccessSubmit, UserSuccessShare, Error }

        /// <summary>
        /// Return Datetime in utc from epoch
        /// </summary>
        /// <returns></returns>
        public static DateTime FromUnixTimeToUtc(int unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        public static DateTime FromUnixTimeToLocal(int unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        /// <summary>
        /// Return utc seconds / unixTime since epoch (01/01/1970)
        /// </summary>
        /// <returns></returns>
        public static int SecondsSinceEpochUTC()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int second = (int)t.TotalSeconds;
            return second;
        }

        public static int SecondsSinceEpochUTC(DateTime datetime)
        {
            TimeSpan t = datetime - new DateTime(1970, 1, 1);
            int second = (int)t.TotalSeconds;
            return second;
        }

        public const string LowerCaseAlphabet = "abcdefghijklmnopqrstuvwyxz";
        public const string UpperCaseAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string Number = "1234567890";

        public static string GenerateRandomString(int length, Random rng, bool includeUpper = true, bool includeLower = true, bool includenumber = true)
        {
            var alphabet = "";

            if (includeUpper)
                alphabet = alphabet + UpperCaseAlphabet;
            if (includeLower)
                alphabet = alphabet + LowerCaseAlphabet;
            if (includenumber)
                alphabet = alphabet + Number;

            return GenerateRandomString(length, rng, alphabet);
        }

        public static string GenerateRandomString(int length, Random rng, string alphabet)
        {
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = alphabet[rng.Next(alphabet.Length)];
            }
            return new string(chars);
        }

        public static string GenerateUniqueString(int length)
        {
            const string alphanumericCharacters =
                UpperCaseAlphabet +
                LowerCaseAlphabet +
                Number;
            return GenerateUniqueString(length, alphanumericCharacters);
        }

        public static string GenerateUniqueString(int length, IEnumerable<char> characterSet)
        {
            if (length < 0)
                throw new ArgumentException("length must not be negative", "length");
            if (length > int.MaxValue / 8) // 250 million chars ought to be enough for anybody
                throw new ArgumentException("length is too big", "length");
            if (characterSet == null)
                throw new ArgumentNullException("characterSet");
            var characterArray = characterSet.Distinct().ToArray();
            if (characterArray.Length == 0)
                throw new ArgumentException("characterSet must not be empty", "characterSet");

            var bytes = new byte[length * 8];
            new RNGCryptoServiceProvider().GetBytes(bytes);
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                ulong value = BitConverter.ToUInt64(bytes, i * 8);
                result[i] = characterArray[value % (uint)characterArray.Length];
            }
            return new string(result);
        }


    }
}
