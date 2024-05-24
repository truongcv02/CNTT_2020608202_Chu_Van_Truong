using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLXe.Models;

namespace QLXe.Areas.Admin.Controllers
{
    public class ChartsController : Controller
    {
        QLXeDB db = new QLXeDB();

        // GET: Admin/Chart
        public ActionResult Index()
        {
            // Lấy tổng giá trị nhập hàng theo từng tháng của 6 tháng gần nhất từ bảng SanPham_NhaSanXuat
            var totalImportByMonth = new decimal?[6];
            var currentDate = DateTime.Now;
            for (int i = 0; i < 6; i++)
            {
                var month = currentDate.AddMonths(-i);
                totalImportByMonth[i] = db.DonNhapHang
                    .Where(s => s.NgayNhap != null && s.NgayNhap.Year == month.Year && s.NgayNhap.Month == month.Month)
                    .Sum(s => s.SanPham.GiaNhap * s.SoLuong);
            }
            Array.Reverse(totalImportByMonth); // Đảo ngược mảng tổng giá trị nhập hàng

            // Lấy tổng giá trị bán hàng theo từng tháng của 6 tháng gần nhất từ bảng DonHang
            var totalSaleByMonth = new decimal?[6];
            for (int i = 0; i < 6; i++)
            {
                var month = currentDate.AddMonths(-i);
                totalSaleByMonth[i] = db.DonHang
                    .Where(d => d.NgayGiaoHang != null && d.TinhTrang == 3 && d.NgayGiaoHang.Value.Year == month.Year && d.NgayGiaoHang.Value.Month == month.Month)
                    .Sum(d => d.TongGiaTri);
            }
            Array.Reverse(totalSaleByMonth); // Đảo ngược mảng tổng giá trị bán hàng

            ViewBag.TotalImportByMonth = totalImportByMonth;
            ViewBag.TotalSaleByMonth = totalSaleByMonth;

            return View();
        }
    }
}
