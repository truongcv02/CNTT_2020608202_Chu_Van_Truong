using QLXe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
namespace QLXe.Areas.Admin.Controllers
{
	public class LoginController : Controller
	{
		// GET: Admin/Login
		QLXeDB db = new QLXeDB();
		// GET: Admin/Login
		public ActionResult Index()
		{
			if (Session["Hoten"] != null)
			{
				string a = Session["Hoten"].ToString();
				if (a == "admin")
				{
					return RedirectToAction("Index", "Home");
				}
			}
			if (TempData["TaoTaiKhoan"] != null)
			{
				ViewBag.TaoTaiKhoan = TempData["TaoTaiKhoan"].ToString();
			}
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Index(string username, string password)
		{
			if (ModelState.IsValid)
			{
				var user = db.TaiKhoan.FirstOrDefault(u => (u.Username.Equals(username) || u.KhachHang.Email.Equals(username)) && u.Password.Equals(password));

				if (user != null && user.KhachHang.Ten == "admin")
				{
					ViewBag.Tongdonhang = db.DonHang.Count();
					ViewBag.Tongdoanhthu = db.DonHang.Sum(s => s.TongGiaTri);
					ViewBag.Tongtaikhoan = db.TaiKhoan.Count();
					ViewBag.Tongsosanphamban = db.ChiTietDonHang.Sum(s => s.SoLuong);
					Session["Hoten"] = user.Username;
					Session["ID_KhachHang"] = user.ID_KhachHang;
					return RedirectToAction("Index", "Home");
				}
				else
				{
					if (user != null && user.Khoa == true)
					{
						Session["Hoten"] = user.Username;
						Session["ID_KhachHang"] = user.ID_KhachHang;
						return RedirectToAction("Index", "Home", new { area = "", namespaces = new[] { "QLXe.Controllers" } });
					}
					if (user != null && user.Khoa==false)
					{
						ViewBag.error = "Tài khoản đã bị khóa";
					}	
					else
					{
						ViewBag.error = "Tài khoản hoặc mật khẩu sai";
					}
					
				}
			}

			return View();
		}
		[HttpPost]
		public ActionResult Repassword(string email)
		{
			if (ModelState.IsValid)
			{
				var khachHang = db.KhachHang.FirstOrDefault(s => s.Email == email);
				if (khachHang != null)
				{
					// Tạo mật khẩu ngẫu nhiên
					string newPassword = GenerateRandomPassword();

					// Lưu mật khẩu mới vào cơ sở dữ liệu
					khachHang.TaiKhoan.FirstOrDefault(s=>s.ID_KhachHang==khachHang.ID).Password = newPassword; // Giả sử 'Password' là trường lưu mật khẩu
					db.SaveChanges();

					// Gửi email chứa mật khẩu mới
					string subject = "Lấy lại mật khẩu";
					string body = $"Mật khẩu mới của bạn là: {newPassword}";
					QLXe.Common.common.SendMail("ShopOnline", subject, body, khachHang.Email);

					return Json(new { success = true, message = "Mật khẩu mới đã được gửi vào Email" });
				}
				return Json(new { success = false, message = "Tài khoản không tồn tại" });
			}
			return View();
		}

		private string GenerateRandomPassword(int length = 8)
		{
			const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
			StringBuilder sb = new StringBuilder();
			Random rnd = new Random();

			while (sb.Length < length)
			{
				sb.Append(validChars[rnd.Next(validChars.Length)]);
			}

			return sb.ToString();
		}

	}
}