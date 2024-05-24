using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLXe.Models;
using PagedList;
namespace QLXe.Areas.Admin.Controllers
{
    public class MauSacsController : Controller
    {
        private QLXeDB db = new QLXeDB();

        // GET: Admin/MauSacs
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            var mauSac = db.MauSac.Include(s => s.SanPham);
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            ViewBag.currentFilter = SearchString;
            if (SearchString != null)
            {
                mauSac = mauSac.Where(s => s.TenMau.Contains(SearchString));
            }
            mauSac = mauSac.OrderBy(s => s.TenMau);
            int PageSize = 5;
            int PageNumber = (page ?? 1);
            return View(mauSac.ToPagedList(PageNumber, PageSize));
        }


        // GET: Admin/MauSacs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MauSac mauSac = db.MauSac.Find(id);
            if (mauSac == null)
            {
                return HttpNotFound();
            }
            return View(mauSac);
        }

        // GET: Admin/MauSacs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/MauSacs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TenMau")] MauSac mauSac)
        {
            if (ModelState.IsValid)
            {
                db.MauSac.Add(mauSac);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mauSac);
        }

        // GET: Admin/MauSacs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MauSac mauSac = db.MauSac.Find(id);
            if (mauSac == null)
            {
                return HttpNotFound();
            }
            return View(mauSac);
        }

        // POST: Admin/MauSacs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TenMau")] MauSac mauSac)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mauSac).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mauSac);
        }

        // GET: Admin/MauSacs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MauSac mauSac = db.MauSac.Find(id);
            if (mauSac == null)
            {
                return HttpNotFound();
            }
            return View(mauSac);
        }

        // POST: Admin/MauSacs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MauSac mauSac = db.MauSac.Find(id);
            db.MauSac.Remove(mauSac);
            db.SaveChanges();
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
