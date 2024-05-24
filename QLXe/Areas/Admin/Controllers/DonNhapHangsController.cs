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
    public class DonNhapHangsController : Controller
    {
        private QLXeDB db = new QLXeDB();

        // GET: Admin/DonNhapHangs
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            var donNhapHang = db.DonNhapHang.Include(d => d.NhaSanXuat).Include(d => d.SanPham);
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
                donNhapHang = donNhapHang.Where(s => s.SanPham.TenXe.Contains(SearchString));
            }
            donNhapHang = donNhapHang.OrderByDescending(s => s.NgayNhap);
            int PageSize = 5;
            int PageNumber = (page ?? 1);
            return View(donNhapHang.ToPagedList(PageNumber, PageSize));
        }

        // GET: Admin/DonNhapHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonNhapHang donNhapHang = db.DonNhapHang.Find(id);
            if (donNhapHang == null)
            {
                return HttpNotFound();
            }
            return View(donNhapHang);
        }

        // GET: Admin/DonNhapHangs/Create
        public ActionResult Create()
        {
            ViewBag.ID_NhaSanXuat = new SelectList(db.NhaSanXuat, "ID", "TenHang");
            ViewBag.ID_SanPham = new SelectList(db.SanPham, "ID", "TenXe");
            return View();
        }

        // POST: Admin/DonNhapHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SoLuong,NgayNhap,ID_SanPham,ID_NhaSanXuat")] DonNhapHang donNhapHang)
        {
            if (ModelState.IsValid)
            {
                var sanPham = db.SanPham.FirstOrDefault(s => s.ID == donNhapHang.ID_SanPham);
                sanPham.SoLuong += donNhapHang.SoLuong;
                db.DonNhapHang.Add(donNhapHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_NhaSanXuat = new SelectList(db.NhaSanXuat, "ID", "TenHang", donNhapHang.ID_NhaSanXuat);
            ViewBag.ID_SanPham = new SelectList(db.SanPham, "ID", "TenXe", donNhapHang.ID_SanPham);
            return View(donNhapHang);
        }

        // GET: Admin/DonNhapHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonNhapHang donNhapHang = db.DonNhapHang.Find(id);
            if (donNhapHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_NhaSanXuat = new SelectList(db.NhaSanXuat, "ID", "TenHang", donNhapHang.ID_NhaSanXuat);
            ViewBag.ID_SanPham = new SelectList(db.SanPham, "ID", "TenXe", donNhapHang.ID_SanPham);
            return View(donNhapHang);
        }

        // POST: Admin/DonNhapHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SoLuong,NgayNhap,ID_SanPham,ID_NhaSanXuat")] DonNhapHang donNhapHang)
        {
            if (ModelState.IsValid)
            {
                var donNhap = db.DonNhapHang.FirstOrDefault(s => s.ID == donNhapHang.ID);
                int idSanPham = db.DonNhapHang.FirstOrDefault(s => s.ID == donNhapHang.ID).ID_SanPham;
                if (donNhapHang.ID_SanPham != idSanPham)
				{
                    var sanPhamTruoc = db.SanPham.FirstOrDefault(s => s.ID == idSanPham);
                    var sanPhamSau = db.SanPham.FirstOrDefault(s => s.ID == donNhapHang.ID_SanPham);
                    sanPhamTruoc.SoLuong -= donNhap.SoLuong;
                    sanPhamSau.SoLuong += donNhapHang.SoLuong;
                }               
                else
				{
                    var sanPham = db.SanPham.FirstOrDefault(s => s.ID == donNhapHang.ID_SanPham);
                    sanPham.SoLuong = sanPham.SoLuong + donNhapHang.SoLuong - donNhap.SoLuong;
                }
                donNhap.ID_SanPham = donNhapHang.ID_SanPham;
                donNhap.ID_NhaSanXuat = donNhapHang.ID_NhaSanXuat;
                donNhap.SoLuong = donNhapHang.SoLuong;
                donNhap.NgayNhap = donNhapHang.NgayNhap;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_NhaSanXuat = new SelectList(db.NhaSanXuat, "ID", "TenHang", donNhapHang.ID_NhaSanXuat);
            ViewBag.ID_SanPham = new SelectList(db.SanPham, "ID", "TenXe", donNhapHang.ID_SanPham);
            return View(donNhapHang);
        }

        // GET: Admin/DonNhapHangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonNhapHang donNhapHang = db.DonNhapHang.Find(id);
            if (donNhapHang == null)
            {
                return HttpNotFound();
            }
            return View(donNhapHang);
        }

        // POST: Admin/DonNhapHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DonNhapHang donNhapHang = db.DonNhapHang.Find(id);
            db.DonNhapHang.Remove(donNhapHang);
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
