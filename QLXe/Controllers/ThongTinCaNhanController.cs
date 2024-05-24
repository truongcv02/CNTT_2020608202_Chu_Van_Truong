using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLXe.Models;
namespace QLXe.Controllers
{
    public class ThongTinCaNhanController : Controller
    {
        QLXeDB db = new QLXeDB();
        // GET: ThongTinCaNhan
        public ActionResult Index()
        {
            int id = (int)Session["ID_KhachHang"];
            var khachHang = db.KhachHang.FirstOrDefault(s => s.ID == id);
            return View(khachHang);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "ID,Ten,Ho,Email,DienThoai,DiaChi")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                if(db.KhachHang.Any(s=>s.Email==khachHang.Email && s.ID != khachHang.ID))
				{
                    TempData["SuaTT"] = "Email đã được dùng cho tài khoản khác!";
                    return RedirectToAction("Index");
                }                    
                db.Entry(khachHang).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuaTT"] = "Đã sửa thông tin";
                return RedirectToAction("Index");
            }
            TempData["SuaTT"] = "Có lỗi khi sửa";
            return RedirectToAction("Index");
        }
    }
}