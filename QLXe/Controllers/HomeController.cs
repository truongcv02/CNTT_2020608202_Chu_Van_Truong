using QLXe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using System.Data;


namespace QLXe.Controllers
{
    public class HomeController : Controller
    {
        private QLXeDB db = new QLXeDB();
        // GET: Home
        public ActionResult Index()
        {
            var slide = db.Slide.Include(s => s.SanPham).ToList();
            return View(slide);
        }
        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            var danhMuc = db.DanhMuc.Include(s => s.DanhMuc2).Include(s => s.DanhMuc1).Include(s => s.SanPham).ToList();
            return PartialView(danhMuc);
        }
        [ChildActionOnly]
        public ActionResult ProductHome()
        {
            var danhMuc = db.DanhMuc.Where(d => d.SanPham.Any()).Include(s => s.DanhMuc1).Include(s => s.DanhMuc2).Include(s => s.SanPham).ToList();
            return PartialView(danhMuc);
        }

        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult SendMail(string TieuDe, string Email, string NoiDung)
        {
            QLXe.Common.common.SendMail("ShopOnline", TieuDe,"Bạn nhận được phản hồi từ: "+Email+"<br/>"+ NoiDung, "motorcycle1234321@gmail.com");
            TempData["sendmail"] = "Cảm ơn bạn đã đóng góp, chúng tôi đã nhận được thư của bạn. Vui lòng chờ phản hồi";
            return RedirectToAction("Contact");
        }
    }
}