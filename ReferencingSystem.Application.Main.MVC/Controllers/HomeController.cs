using ReferencingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReferencingSystem.Application.Main.MVC.Controllers
{
    public class HomeController : Controller
    {
        private RsContext db = new RsContext();
        public ActionResult Index()
        {
            var page = db.Pages.Where(x => x.Title.Equals("home")).FirstOrDefault();
            if (page == null)
                page = new Pages() { Title = "Home", Content = "no homepage found" };
            return View(page);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}