using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLXe.Models;

namespace QLXe.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        QLXeDB db = new QLXeDB();
        // GET: Admin/Home
        public ActionResult Index()
        {
            if (Session["Hoten"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.Tongdonhang = db.DonHang.Count();
                ViewBag.Tongdoanhthu = db.DonHang.Sum(s => s.TongGiaTri);
                ViewBag.Tongtaikhoan = db.TaiKhoan.Count();
                DateTime now = DateTime.Now;
                int currentMonth = now.Month;
                int currentYear = now.Year;
                ViewBag.Tongdonhangdanggiao = db.DonHang.Where(s => s.TinhTrang == 2).Count();
                ViewBag.Tongtaikhoanmoi = db.TaiKhoan.Count(s => s.NgayLap.HasValue && s.NgayLap.Value.Month == currentMonth && s.NgayLap.Value.Year == currentYear);
                ViewBag.Tongsosanphamban = db.ChiTietDonHang.Sum(s => s.SoLuong);
                ViewBag.Tonggiohang = db.GioHang.Count();
                // Get the current month and year

                decimal totalRevenue = db.DonHang.Where(s => s.NgayGiaoHang.Value.Month == currentMonth && s.NgayGiaoHang.Value.Year == currentYear && s.TinhTrang==3).Sum(s => (decimal?)s.TongGiaTri) ?? 0; // Handle nullable decimal

                decimal totalCost = db.ChiTietDonHang.Where(s => s.DonHang.NgayGiaoHang.Value.Month == currentMonth && s.DonHang.NgayGiaoHang.Value.Year == currentYear && s.DonHang.TinhTrang==3).Sum(s => (decimal?)(s.SoLuong * s.SanPham.GiaNhap)) ?? 0;
                // Calculate net revenue
                ViewBag.DoanhThu = totalRevenue- totalCost;
                return View();
            }
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
        public ActionResult Logout2()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home", new { area = "", namespaces = new[] { "QLXe.Controllers" } });
        }
        public ActionResult GetChartData()
        {
            List<DateTime> recentMonths = new List<DateTime>();

            // Lấy tháng hiện tại
            DateTime currentMonth = DateTime.Now;

            // Thêm tháng hiện tại vào danh sách
            recentMonths.Add(currentMonth);

            // Thêm 11 tháng trước đó vào danh sách
            for (int i = 1; i <= 11; i++)
            {
                DateTime previousMonth = currentMonth.AddMonths(-i);
                recentMonths.Add(previousMonth);
            }
            var threerecentMonthsLabels = recentMonths.Take(3).Select(dt => dt.ToString("MM/yyyy")).ToArray();
            var sixrecentMonthsLabels = recentMonths.Take(6).Select(dt => dt.ToString("MM/yyyy")).ToArray();
            var recentMonthsLabels = recentMonths.Take(12).Select(dt => dt.ToString("MM/yyyy")).ToArray();
            var allrecentMonthsLabels = recentMonths.Take(15).Select(dt => dt.ToString("MM/yyyy")).ToArray();
            var listbrand = db.NhaSanXuat.ToList();
            foreach (var i in listbrand)
            {

            }
            //lấy dữ liệu của các hãng vào một mảng 
            var salesData = new
            {
                threeMonths = new
                {
                    labels = threerecentMonthsLabels.Reverse(),
                    nike = new[] { 1000, 1500, 2000 },
                    adidas = new[] { 1200, 1700, 1800 },
                    puma = new[] { 900, 1400, 1500 }
                },
                sixMonths = new
                {
                    labels = sixrecentMonthsLabels.Reverse(),
                    nike = new[] { 1000, 1500, 2000, 1800, 2200, 2500 },
                    adidas = new[] { 1200, 1700, 1800, 1600, 2100, 2300 },
                    puma = new[] { 900, 1400, 1500, 1300, 1700, 1900 }
                },
                oneYear = new
                {
                    labels = recentMonthsLabels.Reverse(),
                    nike = new[] { 1000, 1500, 2000, 1800, 2200, 2500, 2200, 2000, 1800, 2100, 2400, 2700 },
                    adidas = new[] { 1200, 1700, 1800, 1600, 2100, 2300, 2000, 1900, 1800, 1900, 2300, 2500 },
                    puma = new[] { 900, 1400, 1500, 1300, 1700, 1900, 1600, 1500, 1400, 1500, 1700, 1900 }
                },
                all = new
                {
                    labels = allrecentMonthsLabels.Reverse(),
                    nike = new[] { 1000, 1500, 2000, 1800, 2200, 2500, 2200, 2000, 1800, 2100, 2400, 2700 },
                    adidas = new[] { 1200, 1700, 1800, 1600, 2100, 2300, 2000, 1900, 1800, 1900, 2300, 2500 },
                    puma = new[] { 900, 1400, 1500, 1300, 1700, 1900, 1600, 1500, 1400, 1500, 1700, 1900 }
                }
            };

            return Json(salesData);
        }
        [ChildActionOnly]
        public ActionResult thongbao()
        {
            ViewBag.donhang = db.DonHang.Where(s => s.TinhTrang == 1).Count();
            ViewBag.taikhoan = db.TaiKhoan.Where(s => s.NgayLap == DateTime.Today).Count();
            return PartialView();
        }
    }
}


