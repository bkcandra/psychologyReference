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
    public class FilesController : Controller
    {
        private RsContext db = new RsContext();

        // GET: Files
        public async Task<ActionResult> Index()
        {
            return View(await db.WebAssets.ToListAsync());
        }

        // GET: Files/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WebAssets webAssets = await db.WebAssets.FindAsync(id);
            if (webAssets == null)
            {
                return HttpNotFound();
            }
            return View(webAssets);
        }

        // GET: Files/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FileName,Title,Description,Extension,Size,File,CreatedUTC,CreatedBy,ModifiedUTC,ModifiedBy")] WebAssets webAssets)
        {
            if (ModelState.IsValid)
            {
                db.WebAssets.Add(webAssets);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(webAssets);
        }

        // GET: Files/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WebAssets webAssets = await db.WebAssets.FindAsync(id);
            if (webAssets == null)
            {
                return HttpNotFound();
            }
            return View(webAssets);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FileName,Title,Description,Extension,Size,File,CreatedUTC,CreatedBy,ModifiedUTC,ModifiedBy")] WebAssets webAssets)
        {
            if (ModelState.IsValid)
            {
                db.Entry(webAssets).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(webAssets);
        }

        // GET: Files/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WebAssets webAssets = await db.WebAssets.FindAsync(id);
            if (webAssets == null)
            {
                return HttpNotFound();
            }
            return View(webAssets);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            WebAssets webAssets = await db.WebAssets.FindAsync(id);
            db.WebAssets.Remove(webAssets);
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
