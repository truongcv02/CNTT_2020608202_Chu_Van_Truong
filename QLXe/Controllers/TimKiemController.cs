using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLXe.Models;

namespace QLXe.Controllers
{
    public class TimKiemController : Controller
    {
        QLXeDB db = new QLXeDB();
        // GET: TimKiem
        public ActionResult Index(string tenSanPham, string priceOrder, string khuyenMai)
        {
                         
            var sanPhams = db.SanPham.AsQueryable();
            if (khuyenMai == "30")
            {
                sanPhams = sanPhams.Where(s => s.KhuyenMai.PhanTramGiamGia >= 30);
            }
            if (khuyenMai == "10")
            {
                sanPhams = sanPhams.Where(s => s.KhuyenMai.PhanTramGiamGia >= 10);
            }
            if (!string.IsNullOrEmpty(tenSanPham))
            {
                sanPhams = sanPhams.Where(s => s.TenXe.Contains(tenSanPham));
            }
            
            TempData["tenSP"] = tenSanPham;
            switch (priceOrder)
            {
                case "asc":
                    sanPhams = sanPhams.OrderBy(s => s.GiaBan*(100-s.KhuyenMai.PhanTramGiamGia)/100);
                    break;
                case "desc":
                    sanPhams = sanPhams.OrderByDescending(s => s.GiaBan * (100 - s.KhuyenMai.PhanTramGiamGia) / 100);
                    break;
                default:
                    break;
            }
            return View(sanPhams);
        }
    }
}