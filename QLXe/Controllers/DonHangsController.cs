using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLXe.Models;
using System.Data.Entity;
using System.Net;
using System.Data;

namespace QLXe.Controllers
{
    public class DonHangsController : Controller
    {
        public QLXeDB db = new QLXeDB();
        public ActionResult Index(int? id, int? tt)
		{           
            if(tt==0)
			{
                ViewBag.ID_KhachHang = id;
                var donHang = db.DonHang.Include(s => s.ChiTietDonHang).Include(s => s.KhachHang).Where(s => s.ID_KhachHang == id).OrderBy(s => s.TinhTrang).ToList();
                return View(donHang);
            }
            else
			{
                ViewBag.ID_KhachHang = id;
                var donHang = db.DonHang.Where(s=>s.TinhTrang==tt).Include(s => s.ChiTietDonHang).Include(s => s.KhachHang).Where(s => s.ID_KhachHang == id).OrderBy(s => s.TinhTrang).ToList();
                return View(donHang);
            }                
		}

        [HttpPost]
        public ActionResult ToggleLock(int id)
        {
            
            var DonHang = db.DonHang.Find(id);
            var ctdh = db.DonHang.Find(id).ChiTietDonHang.ToList();
            foreach(var item in ctdh)
			{
                item.SanPham.SoLuong += item.SoLuong;
                db.SaveChanges();
            }                
            if (DonHang != null)
            {
                DonHang.TinhTrang = 4;

                db.SaveChanges();
                return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
            }
            return Json(new { success = false, message = "DonHang không tồn tại" });
        }
    }
}