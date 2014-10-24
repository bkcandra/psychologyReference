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

namespace ReferencingSystem.Application.Admin.MVC.Controllers
{
    public class PagesController : Controller
    {
        private RsContext db = new RsContext();

        // GET: Pages
        public async Task<ActionResult> Index()
        {
            return View(await db.Pages.ToListAsync());
        }

        // GET: Pages/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pages pages = await db.Pages.FindAsync(id);
            if (pages == null)
            {
                return HttpNotFound();
            }
            return View(pages);
        }

        // GET: Pages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Title,TemplateId,MetaTag,MetaDescription,CreatedUTC,ModifiedUTC,CreatedBy,ModifiedBy,Content,Published,ModifiedDateTime")] Pages pages)
        {
            if (ModelState.IsValid)
            {
                db.Pages.Add(pages);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pages);
        }

        // GET: Pages/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pages pages = await db.Pages.FindAsync(id);
            if (pages == null)
            {
                return HttpNotFound();
            }
            return View(pages);
        }

        // POST: Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Title,TemplateId,MetaTag,MetaDescription,CreatedUTC,ModifiedUTC,CreatedBy,ModifiedBy,Content,Published,ModifiedDateTime")] Pages pages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pages).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pages);
        }

        // GET: Pages/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pages pages = await db.Pages.FindAsync(id);
            if (pages == null)
            {
                return HttpNotFound();
            }
            return View(pages);
        }

        // POST: Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Pages pages = await db.Pages.FindAsync(id);
            db.Pages.Remove(pages);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
