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
    public class SlidesController : Controller
    {
        private QLXeDB db = new QLXeDB();

        // GET: Admin/Slides
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            var slide = db.Slide.Select(p=>p);
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
                slide = slide.Where(s => s.SanPham.TenXe.Contains(SearchString));
            }
            slide = slide.OrderBy(s => s.SanPham.TenXe);
            int PageSize = 5;
            int PageNumber = (page ?? 1);
            return View(slide.ToPagedList(PageNumber, PageSize));
        }

        // GET: Admin/Slides/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide slide = db.Slide.Find(id);
            if (slide == null)
            {
                return HttpNotFound();
            }
            return View(slide);
        }

        // GET: Admin/Slides/Create
        public ActionResult Create()
        {
            ViewBag.ID_SanPham = new SelectList(db.SanPham, "ID", "TenXe");
            return View();
        }

        // POST: Admin/Slides/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection form)
        {
            Slide slide = new Slide();
            slide.ID_SanPham = int.Parse(form["ID_SanPham"]);
            try
			{
                if (ModelState.IsValid)
                {
                    
                    slide.Slide1 = "";
                    var k = Request.Files["InputFile"];

                    if (k != null && k.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(k.FileName);
                        string UpLoad = Server.MapPath("~/wwwroot/Images/" + FileName);
                        k.SaveAs(UpLoad);
                        slide.Slide1 = FileName;
                    }
                    db.Slide.Add(slide);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch(Exception ex)
			{
                ViewBag.Error = "Lỗi nhập dữ liệu! " + ex.Message;
                ViewBag.ID_SanPham = new SelectList(db.SanPham, "ID", "TenXe", slide.ID_SanPham);
                return View(slide);
            }                 
        }

        // GET: Admin/Slides/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide slide = db.Slide.Find(id);
            if (slide == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_SanPham = new SelectList(db.SanPham, "ID", "TenXe", slide.ID_SanPham);
            return View(slide);
        }

        // POST: Admin/Slides/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection form)
        {
            int ID = int.Parse(form["ID"]);

            Slide sl = db.Slide.FirstOrDefault(s => s.ID == ID);
            sl.ID_SanPham = int.Parse(form["ID_SanPham"]);
            if (ModelState.IsValid)
            {
                var k = Request.Files["InputFile"];
                if (k != null && k.ContentLength > 0)
                {
                    string FileName = System.IO.Path.GetFileName(k.FileName);
                    string UpLoad = Server.MapPath("~/wwwroot/Images/" + FileName);
                    k.SaveAs(UpLoad);
                    sl.Slide1 = FileName;
                }
                db.Entry(sl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_SanPham = new SelectList(db.SanPham, "ID", "TenXe", ID);
            return View(sl);
        }

        // GET: Admin/Slides/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide slide = db.Slide.Find(id);
            if (slide == null)
            {
                return HttpNotFound();
            }
            return View(slide);
        }

        // POST: Admin/Slides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slide slide = db.Slide.Find(id);
            db.Slide.Remove(slide);
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
