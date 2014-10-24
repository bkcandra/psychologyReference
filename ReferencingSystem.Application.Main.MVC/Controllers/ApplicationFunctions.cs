using ReferencingSystem.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ReferencingSystem.Application.Main.MVC.Controllers
{
    public class ApplicationFunctions
    {
        public static string GetWebRefStatus(int status)
        {
            if (Enum.IsDefined(typeof(SystemConstants.Status), status))
                return ConfigurationManager.AppSettings[((SystemConstants.Status)status).ToString()];
            else
                return string.Empty;
        }
    }
}