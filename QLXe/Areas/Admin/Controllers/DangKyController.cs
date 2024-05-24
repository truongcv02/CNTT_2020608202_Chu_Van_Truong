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
    public class DangKyController : Controller
    {
        // GET: Admin/DangKy
        QLXeDB db = new QLXeDB();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)
		{
            if (form["Email"]!=null && form["Ten"]!= null && form["Ho"] != null && form["DiaChi"] != null && form["SDT"] != null && form["FullName"] != null && form["Password"] != null)
            {
                string email = form["Email"];
                string ten = form["Ten"];
                string ho = form["Ho"];
                string diachi = form["DiaChi"];
                string sdt = form["SDT"];
                string tendn = form["FullName"];
                if (db.KhachHang.Any(kh => kh.Email == email) || db.TaiKhoan.Any(kh=>kh.Username == tendn))
                {
                    ViewBag.TaoTaiKhoan = "Email đã được sử dụng.Vui lòng chọn một email khác.";
                    return View();
                }
                var khachHang = new KhachHang
                {
                    Ten = ten,
                    Ho = ho,
                    DienThoai = sdt,
                    DiaChi = diachi,
                    Email = email
                };
                db.KhachHang.Add(khachHang);
                db.SaveChanges();
                var taiKhoan = new TaiKhoan();
                taiKhoan.ID_KhachHang = khachHang.ID;
                taiKhoan.Username = form["FullName"];
                taiKhoan.Password = form["Password"];
                taiKhoan.Khoa = true;
                taiKhoan.NgayLap = DateTime.Now;
                db.TaiKhoan.Add(taiKhoan);
                db.SaveChanges();
                TempData["TaoTaiKhoan"] = "Tài khoản đã được tạo thành công!";
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }
            ViewBag.Error = "Nhập thiếu dữ liệu";
            return View();
        }
    }
}