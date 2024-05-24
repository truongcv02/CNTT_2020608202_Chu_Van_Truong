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
    public class KhuyenMaisController : Controller
    {
        private QLXeDB db = new QLXeDB();

        // GET: Admin/KhuyenMais
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            var khuyenMai = db.KhuyenMai.Include(s=>s.SanPham);
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
                khuyenMai = khuyenMai.Where(s => s.TenKhuyenMai.Contains(SearchString));
            }
            khuyenMai = khuyenMai.OrderBy(s => s.TenKhuyenMai);
            int PageSize = 5;
            int PageNumber = (page ?? 1);
            return View(khuyenMai.ToPagedList(PageNumber, PageSize));
        }

        // GET: Admin/KhuyenMais/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhuyenMai khuyenMai = db.KhuyenMai.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenMai);
        }

        // GET: Admin/KhuyenMais/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/KhuyenMais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TenKhuyenMai,PhanTramGiamGia")] KhuyenMai khuyenMai)
        {
            if (ModelState.IsValid)
            {
                db.KhuyenMai.Add(khuyenMai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(khuyenMai);
        }

        // GET: Admin/KhuyenMais/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhuyenMai khuyenMai = db.KhuyenMai.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenMai);
        }

        // POST: Admin/KhuyenMais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TenKhuyenMai,PhanTramGiamGia")] KhuyenMai khuyenMai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khuyenMai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khuyenMai);
        }

        // GET: Admin/KhuyenMais/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhuyenMai khuyenMai = db.KhuyenMai.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenMai);
        }

        // POST: Admin/KhuyenMais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KhuyenMai khuyenMai = db.KhuyenMai.Find(id);
            db.KhuyenMai.Remove(khuyenMai);
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
