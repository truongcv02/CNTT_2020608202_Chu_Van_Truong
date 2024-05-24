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
    public class SanPhamsController : Controller
    {
        private QLXeDB db = new QLXeDB();

        // GET: Admin/SanPhams
        public ActionResult Index(string SearchString, string currentFilter,int? page, string sortOrder)
        {
            var sanPham = db.SanPham.Include(s => s.DanhMuc).Include(s => s.KhuyenMai).Include(s => s.MauSac);
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
                sanPham = sanPham.Where(s => s.TenXe.Contains(SearchString));
            }
            sanPham = sanPham.OrderBy(s => s.TenXe);
            int PageSize = 5;
            int PageNumber = (page ?? 1);
            ViewBag.SapXepTheoTen = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SapXepTheoGia = sortOrder == "Gia" ? "gia_desc" : "Gia";
            switch (sortOrder)
            {
                case "name_desc":
                    sanPham = sanPham.OrderByDescending(s => s.TenXe);
                    break;
                case "gia_desc":
                    sanPham = sanPham.OrderBy(s => s.GiaBan);
                    break;
                case "Gia":
                    sanPham = sanPham.OrderByDescending(s => s.GiaBan);
                    break;
                default:
                    sanPham = sanPham.OrderBy(s => s.TenXe);
                    break;
            }
            return View(sanPham.ToPagedList(PageNumber, PageSize));
        }

        // GET: Admin/SanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: Admin/SanPhams/Create
        public ActionResult Create()
        {
            // Chỉ lấy danh sách các danh mục có ID_DanhMuc khác null
            var danhMucList = db.DanhMuc.Where(d => d.ID_DanhMuc != null).ToList();
            // Tạo SelectList từ danh sách đã lọc
            ViewBag.ID_DanhMuc = new SelectList(danhMucList, "ID", "TenDanhMuc");
            ViewBag.ID_MauSac = new SelectList(db.MauSac, "ID", "TenMau");
            ViewBag.ID_KhuyenMai = new SelectList(db.KhuyenMai, "ID", "TenKhuyenMai");
            return View();
        }

        // POST: Admin/SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TenXe,NamSX,ThongSo,GiaNhap,GiaBan,ID_DanhMuc,ID_KhuyenMai,SoLuong,ID_MauSac")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.SanPham.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_MauSac = new SelectList(db.MauSac, "ID", "TenMau", sanPham.ID_MauSac);
            ViewBag.ID_DanhMuc = new SelectList(db.DanhMuc, "ID", "TenDanhMuc", sanPham.ID_DanhMuc);
            ViewBag.ID_KhuyenMai = new SelectList(db.KhuyenMai, "ID", "TenKhuyenMai", sanPham.ID_KhuyenMai);
            return View(sanPham);
        }

        // GET: Admin/SanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_MauSac = new SelectList(db.MauSac, "ID", "TenMau", sanPham.ID_MauSac);
            ViewBag.ID_DanhMuc = new SelectList(db.DanhMuc, "ID", "TenDanhMuc", sanPham.ID_DanhMuc);
            ViewBag.ID_KhuyenMai = new SelectList(db.KhuyenMai, "ID", "TenKhuyenMai", sanPham.ID_KhuyenMai);
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TenXe,NamSX,ThongSo,GiaNhap,GiaBan,ID_DanhMuc,ID_KhuyenMai,SoLuong,ID_MauSac")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_MauSac = new SelectList(db.MauSac, "ID", "TenMau", sanPham.ID_MauSac);
            ViewBag.ID_DanhMuc = new SelectList(db.DanhMuc, "ID", "TenDanhMuc", sanPham.ID_DanhMuc);
            ViewBag.ID_KhuyenMai = new SelectList(db.KhuyenMai, "ID", "TenKhuyenMai", sanPham.ID_KhuyenMai);
            return View(sanPham);
        }

        // GET: Admin/SanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPham.Find(id);
            db.SanPham.Remove(sanPham);
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
