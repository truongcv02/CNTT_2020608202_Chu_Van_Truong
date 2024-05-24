
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLXe.Models;

namespace QLXe.Areas.Admin.Controllers
{
    public class TaiKhoansController : Controller
    {
        private QLXeDB db = new QLXeDB();

        // GET: Admin/TaiKhoans
        public ActionResult Index()
        {
            var taiKhoan = db.TaiKhoan.Include(t => t.KhachHang);
            return View(taiKhoan.ToList());
        }

        // GET: Admin/TaiKhoans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost]
        public ActionResult ToggleLock(int id)
        {
            // Tìm tài khoản dựa trên id và thay đổi trạng thái "Khóa"
            var taiKhoan = db.TaiKhoan.Find(id);
            if (taiKhoan != null)
            {
                taiKhoan.Khoa = !taiKhoan.Khoa;
                bool a = taiKhoan.Khoa;
                db.SaveChanges();
                return Json(new { success = true, a, message = "Cập nhật trạng thái thành công" });
            }
            return Json(new { success = false, message = "Tài khoản không tồn tại" });
        }
    }
}
