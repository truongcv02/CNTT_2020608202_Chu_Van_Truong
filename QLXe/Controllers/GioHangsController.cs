using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLXe.Models;

namespace QLXe.Controllers
{
    public class GioHangsController : Controller
    {
        private QLXeDB db = new QLXeDB();

        // GET: GioHangs
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Lấy ra danh sách sản phẩm trong giỏ hàng của khách hàng có ID là id
            var gioHang = db.GioHang.Where(g => g.ID_KhachHang == id).Include(g => g.KhachHang).Include(g => g.SanPham);
            foreach(var item in gioHang.ToList())
			{
                if(item.SoLuong > item.SanPham.SoLuong)
				{
                    item.SoLuong = item.SanPham.SoLuong;
				}                    
			}
            db.SaveChanges();
            if (gioHang == null)
            {
                return HttpNotFound();
            }
            return View(gioHang.ToList());
        }
        public ActionResult Index2(string ids)
        {
            if (ids == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int id = (int)Session["ID_KhachHang"];
            var idSanPhamArray = ids.Split(',');
            // Lấy ra danh sách sản phẩm trong giỏ hàng của khách hàng có ID là id
            var gioHang = db.GioHang.Where(g => g.ID_KhachHang == id && idSanPhamArray.Contains(g.ID_SanPham.ToString())).Include(g => g.KhachHang).Include(g => g.SanPham).ToList(); ;           
            if (gioHang == null)
            {
                return HttpNotFound();
            }
            return View(gioHang.ToList());
        }
        public ActionResult Index3(string ids)
        {
            if (ids == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int id = (int)Session["ID_KhachHang"];
            var idSanPhamArray = ids.Split(',');
            // Lấy ra danh sách sản phẩm trong giỏ hàng của khách hàng có ID là id
            var gioHang = db.GioHang.Where(g => g.ID_KhachHang == id && idSanPhamArray.Contains(g.ID_SanPham.ToString())).Include(g => g.KhachHang).Include(g => g.SanPham).ToList(); ;
            if (gioHang == null)
            {
                return HttpNotFound();
            }
            return View(gioHang.ToList());
        }
        [HttpPost]
        public ActionResult Update(int id, int? quantity, string type)
        {
            var gioHang = db.GioHang.Find(id);
            var idkh = gioHang.ID_KhachHang;
            if (gioHang != null)
            {
                if (quantity == 0)
                {
                    db.GioHang.Remove(gioHang);
                    db.SaveChanges();

                    // Sau khi xóa sản phẩm, tính lại tổng tiền
                    var gioHangsAfterRemove = db.GioHang.Where(g => g.ID_KhachHang == idkh).ToList();
                    decimal totalAfterRemove = 0;
                    foreach (var item in gioHangsAfterRemove)
                    {
                        totalAfterRemove += item.SoLuong.Value * item.SanPham.GiaBan.Value * (100 - item.SanPham.KhuyenMai.PhanTramGiamGia.Value) / 100;
                    }

                    return Json(new { success = true, removed = true, total = totalAfterRemove, message = "Sản phẩm đã được xóa khỏi giỏ hàng" });
                }
                else
                {
                    if(gioHang.SoLuong>= gioHang.SanPham.SoLuong && type== "increase")
					{
                        gioHang.SoLuong = quantity-1;
                        db.SaveChanges();
                        return Json(new { success = false, removed = false, message = "Vượt quá số lượng trong kho" });
                    }                        
                    gioHang.SoLuong = quantity;
                    db.SaveChanges();

                    // Sau khi cập nhật số lượng, tính lại tổng tiền
                    var gioHangs = db.GioHang.Where(g => g.ID_KhachHang == idkh).ToList();
                    decimal total = 0;
                    foreach (var item in gioHangs)
                    {
                        total += item.SoLuong.Value * item.SanPham.GiaBan.Value * (100 - item.SanPham.KhuyenMai.PhanTramGiamGia.Value) / 100;
                    }
                    var k = db.GioHang.Find(id);
                    decimal thanhTien = k.SoLuong.Value * k.SanPham.GiaBan.Value;

                    return Json(new { success = true, removed = false, thanhTien, total, message = "Cập nhật trạng thái thành công" });
                }
            }
            return Json(new { success = false, message = "Tài khoản không tồn tại" });
        }

    }
}
