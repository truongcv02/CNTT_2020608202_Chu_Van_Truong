
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
    public class HinhAnhsController : Controller
    {
        private QLXeDB db = new QLXeDB();

        // GET: Admin/HinhAnhs
        public ActionResult Index(string SearchString, string currentFilter, int? page, string sortOrder)
        {

            var hinhAnh = db.HinhAnh.Select(p => p);

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
                hinhAnh = hinhAnh.Where(s => s.SanPham.TenXe.Contains(SearchString));
            }
            hinhAnh = hinhAnh.OrderBy(s => s.SanPham.TenXe);
            int PageSize = 5;
            int PageNumber = (page ?? 1);

            ViewBag.SapXepTheoTen = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            switch (sortOrder)
            {
                case "name_desc":
                    hinhAnh = hinhAnh.OrderByDescending(s => s.SanPham.TenXe);
                    break;
                default:
                    hinhAnh = hinhAnh.OrderBy(s => s.SanPham.TenXe);
                    break;
            }
            return View(hinhAnh.ToPagedList(PageNumber, PageSize));
        }

        // GET: Admin/HinhAnhs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HinhAnh hinhAnh = db.HinhAnh.Find(id);
            if (hinhAnh == null)
            {
                return HttpNotFound();
            }
            return View(hinhAnh);
        }

        // GET: Admin/HinhAnhs/Create
        public ActionResult Create()
        {
            var sanPham = db.SanPham.Where(sp => !db.HinhAnh.Any(ha => ha.ID_SanPham == sp.ID)).ToList();
            ViewBag.ID_SanPham = new SelectList(sanPham, "ID", "TenXe");
            return View();
        }

        // POST: Admin/HinhAnhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection form)
        {
            HinhAnh hinhAnh = new HinhAnh();
            hinhAnh.ID_SanPham = int.Parse(form["ID_SanPham"]);
            if (ModelState.IsValid)
            {
                // Khởi tạo tất cả các cột ảnh với giá trị rỗng
                hinhAnh.AnhTruoc = "";
                hinhAnh.AnhTrai = "";
                hinhAnh.AnhSau = "";
                hinhAnh.AnhPhai = "";

                // Xử lý và lưu ảnh cho từng cột
                var fileTruoc = Request.Files["InputAnhTruoc"];
                if (fileTruoc != null && fileTruoc.ContentLength > 0)
                {
                    string fileNameTruoc = System.IO.Path.GetFileName(fileTruoc.FileName);
                    string uploadPathTruoc = Server.MapPath("~/wwwroot/Images/" + fileNameTruoc);
                    fileTruoc.SaveAs(uploadPathTruoc);
                    hinhAnh.AnhTruoc = fileNameTruoc;
                }

                var fileTrai = Request.Files["InputAnhTrai"];
                if (fileTrai != null && fileTrai.ContentLength > 0)
                {
                    string fileNameTrai = System.IO.Path.GetFileName(fileTrai.FileName);
                    string uploadPathTrai = Server.MapPath("~/wwwroot/Images/" + fileNameTrai);
                    fileTrai.SaveAs(uploadPathTrai);
                    hinhAnh.AnhTrai = fileNameTrai;
                }

                var fileSau = Request.Files["InputAnhSau"];
                if (fileSau != null && fileSau.ContentLength > 0)
                {
                    string fileNameSau = System.IO.Path.GetFileName(fileSau.FileName);
                    string uploadPathSau = Server.MapPath("~/wwwroot/Images/" + fileNameSau);
                    fileSau.SaveAs(uploadPathSau);
                    hinhAnh.AnhSau = fileNameSau;
                }

                var filePhai = Request.Files["InputAnhPhai"];
                if (filePhai != null && filePhai.ContentLength > 0)
                {
                    string fileNamePhai = System.IO.Path.GetFileName(filePhai.FileName);
                    string uploadPathPhai = Server.MapPath("~/wwwroot/Images/" + fileNamePhai);
                    filePhai.SaveAs(uploadPathPhai);
                    hinhAnh.AnhPhai = fileNamePhai;
                }

                db.HinhAnh.Add(hinhAnh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_SanPham = new SelectList(db.SanPham, "ID", "TenXe", hinhAnh.ID_SanPham);
            return View(hinhAnh);
        }

        // GET: Admin/HinhAnhs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HinhAnh hinhAnh = db.HinhAnh.Find(id);
            if (hinhAnh == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_SanPham = new SelectList(db.SanPham, "ID", "TenXe", hinhAnh.ID_SanPham);
            return View(hinhAnh);
        }

        // POST: Admin/HinhAnhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection form)
        {
            int ID = int.Parse(form["ID"]);
            HinhAnh uhn = db.HinhAnh.FirstOrDefault(x => x.ID == ID);
            uhn.ID_SanPham = int.Parse(form["ID_SanPham"]);
            if (ModelState.IsValid)
            {


                // Khởi tạo tất cả các cột ảnh với giá trị rỗng
                // Xử lý và lưu ảnh cho từng cột
                var fileTruoc = Request.Files["InputAnhTruoc"];
                if (fileTruoc != null && fileTruoc.ContentLength > 0)
                {
                    string fileNameTruoc = System.IO.Path.GetFileName(fileTruoc.FileName);
                    string uploadPathTruoc = Server.MapPath("~/wwwroot/Images/" + fileNameTruoc);
                    fileTruoc.SaveAs(uploadPathTruoc);
                    uhn.AnhTruoc = fileNameTruoc;
                }

                var fileTrai = Request.Files["InputAnhTrai"];
                if (fileTrai != null && fileTrai.ContentLength > 0)
                {
                    string fileNameTrai = System.IO.Path.GetFileName(fileTrai.FileName);
                    string uploadPathTrai = Server.MapPath("~/wwwroot/Images/" + fileNameTrai);
                    fileTrai.SaveAs(uploadPathTrai);
                    uhn.AnhTrai = fileNameTrai;
                }

                var fileSau = Request.Files["InputAnhSau"];
                if (fileSau != null && fileSau.ContentLength > 0)
                {
                    string fileNameSau = System.IO.Path.GetFileName(fileSau.FileName);
                    string uploadPathSau = Server.MapPath("~/wwwroot/Images/" + fileNameSau);
                    fileSau.SaveAs(uploadPathSau);
                    uhn.AnhSau = fileNameSau;
                }

                var filePhai = Request.Files["InputAnhPhai"];
                if (filePhai != null && filePhai.ContentLength > 0)
                {
                    string fileNamePhai = System.IO.Path.GetFileName(filePhai.FileName);
                    string uploadPathPhai = Server.MapPath("~/wwwroot/Images/" + fileNamePhai);
                    filePhai.SaveAs(uploadPathPhai);
                    uhn.AnhPhai = fileNamePhai;
                }
                db.Entry(uhn).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_SanPham = new SelectList(db.SanPham, "ID", "TenXe", uhn.ID_SanPham);
            return View(uhn);
        }

        // GET: Admin/HinhAnhs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HinhAnh hinhAnh = db.HinhAnh.Find(id);
            if (hinhAnh == null)
            {
                return HttpNotFound();
            }
            return View(hinhAnh);
        }

        // POST: Admin/HinhAnhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HinhAnh hinhAnh = db.HinhAnh.Find(id);
            db.HinhAnh.Remove(hinhAnh);
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