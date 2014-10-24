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
    public class FormsController : Controller
    {
        private RsContext db = new RsContext();

        // GET: Forms
        public async Task<ActionResult> Index()
        {
            return View(await db.RefForm.ToListAsync());
        }

        // GET: Forms/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RefForm refForm = await db.RefForm.FindAsync(id);
            if (refForm == null)
            {
                return HttpNotFound();
            }
            return View(refForm);
        }

        // GET: Forms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Forms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,UserType,Type,Name,Readable,IsActive,Status,CreatedUTC,CreatedBy,ModifiedUTC,ModifiedBy")] RefForm refForm)
        {
            if (ModelState.IsValid)
            {
                db.RefForm.Add(refForm);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(refForm);
        }

        // GET: Forms/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RefForm refForm = await db.RefForm.FindAsync(id);
            if (refForm == null)
            {
                return HttpNotFound();
            }
            return View(refForm);
        }

        // POST: Forms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserType,Type,Name,Readable,IsActive,Status,CreatedUTC,CreatedBy,ModifiedUTC,ModifiedBy")] RefForm refForm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(refForm).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(refForm);
        }

        // GET: Forms/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RefForm refForm = await db.RefForm.FindAsync(id);
            if (refForm == null)
            {
                return HttpNotFound();
            }
            return View(refForm);
        }

        // POST: Forms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            RefForm refForm = await db.RefForm.FindAsync(id);
            db.RefForm.Remove(refForm);
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
