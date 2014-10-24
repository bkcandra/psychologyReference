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
    public class CourseLevelsController : Controller
    {
        private RsContext db = new RsContext();

        // GET: CourseLevels
        public async Task<ActionResult> Index()
        {
            return View(await db.CourseLevel.ToListAsync());
        }

        // GET: CourseLevels/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseLevel courseLevel = await db.CourseLevel.FindAsync(id);
            if (courseLevel == null)
            {
                return HttpNotFound();
            }
            return View(courseLevel);
        }

        // GET: CourseLevels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseLevels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,CreatedUTC,CreatedBy,ModifiedUTC,ModifiedBy")] CourseLevel courseLevel)
        {
            if (ModelState.IsValid)
            {
                db.CourseLevel.Add(courseLevel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(courseLevel);
        }

        // GET: CourseLevels/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseLevel courseLevel = await db.CourseLevel.FindAsync(id);
            if (courseLevel == null)
            {
                return HttpNotFound();
            }
            return View(courseLevel);
        }

        // POST: CourseLevels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,CreatedUTC,CreatedBy,ModifiedUTC,ModifiedBy")] CourseLevel courseLevel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseLevel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(courseLevel);
        }

        // GET: CourseLevels/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseLevel courseLevel = await db.CourseLevel.FindAsync(id);
            if (courseLevel == null)
            {
                return HttpNotFound();
            }
            return View(courseLevel);
        }

        // POST: CourseLevels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CourseLevel courseLevel = await db.CourseLevel.FindAsync(id);
            db.CourseLevel.Remove(courseLevel);
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
