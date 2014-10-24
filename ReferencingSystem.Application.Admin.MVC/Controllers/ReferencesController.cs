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
    public class ReferencesController : Controller
    {
        private RsContext db = new RsContext();

        // GET: References
        public async Task<ActionResult> Index()
        {
            var reference = db.Reference.Include(r => r.AspNetUsers).Include(r => r.RefForm);
            return View(await reference.ToListAsync());
        }

        // GET: References/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference reference = await db.Reference.FindAsync(id);
            if (reference == null)
            {
                return HttpNotFound();
            }
            return View(reference);
        }

        // GET: References/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.FormId = new SelectList(db.RefForm, "Id", "Name");
            return View();
        }

        // POST: References/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FormId,UserId,TransId,Token,RefEmail,RefUserId,Note,IsActive,Status,ExpiryDateUTC,CreatedUTC,CreatedBy,ModifiedUTC,ModifiedBy")] Reference reference)
        {
            if (ModelState.IsValid)
            {
                db.Reference.Add(reference);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", reference.UserId);
            ViewBag.FormId = new SelectList(db.RefForm, "Id", "Name", reference.FormId);
            return View(reference);
        }

        // GET: References/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference reference = await db.Reference.FindAsync(id);
            if (reference == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", reference.UserId);
            ViewBag.FormId = new SelectList(db.RefForm, "Id", "Name", reference.FormId);
            return View(reference);
        }

        // POST: References/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FormId,UserId,TransId,Token,RefEmail,RefUserId,Note,IsActive,Status,ExpiryDateUTC,CreatedUTC,CreatedBy,ModifiedUTC,ModifiedBy")] Reference reference)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reference).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", reference.UserId);
            ViewBag.FormId = new SelectList(db.RefForm, "Id", "Name", reference.FormId);
            return View(reference);
        }

        // GET: References/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference reference = await db.Reference.FindAsync(id);
            if (reference == null)
            {
                return HttpNotFound();
            }
            return View(reference);
        }

        // POST: References/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Reference reference = await db.Reference.FindAsync(id);
            db.Reference.Remove(reference);
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
