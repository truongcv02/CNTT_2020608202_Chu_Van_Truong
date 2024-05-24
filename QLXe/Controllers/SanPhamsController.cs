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
    public class SanPhamsController : Controller
    {
        private QLXeDB db = new QLXeDB();
        public ActionResult Index()
        {
            var danhMuc = db.DanhMuc.Where(d => d.SanPham.Any()).Include(s => s.DanhMuc1).Include(s => s.DanhMuc2).Include(s => s.SanPham).ToList();
            return View(danhMuc);
        }
        // GET: SanPhams
        public ActionResult ChiTietSanPham(int? id)
        {
            SanPham a = db.SanPham.FirstOrDefault(s => s.ID == id);
            var danhMuc = db.DanhMuc.FirstOrDefault(dm => dm.ID == a.ID_DanhMuc);

            if (danhMuc != null)
            {
                // Truy xuất tất cả các sản phẩm trong danh mục này
                var cacSanPhamTrongDanhMuc = db.SanPham.Where(sp => sp.ID_DanhMuc == danhMuc.ID).ToList();

                List<string> tenMauList = new List<string>(); // Khởi tạo danh sách các tên màu
                foreach (var spTrongDanhMuc in cacSanPhamTrongDanhMuc)
                {
                    // Truy xuất đến bảng MauSac để lấy ra TenMau
                    var mauSac = db.MauSac.FirstOrDefault(ms => ms.ID == spTrongDanhMuc.ID_MauSac);
                    if (mauSac != null)
                    {       
                        tenMauList.Add(mauSac.TenMau); // Thêm tên màu vào danh sách
                    }
                }

                ViewBag.tenMauArray = tenMauList.ToArray(); // Chuyển danh sách các tên màu thành mảng và gán vào ViewBag
                // Truy vấn tất cả các hình ảnh từ bảng HinhAnh
                var hinhAnhList = db.HinhAnh.ToList();
                ViewBag.HinhAnhList = hinhAnhList; // Truyền danh sách hình ảnh vào ViewBag
            }

            return View(a);
        }
        public ActionResult TheoDanhMuc1(int? id)
        {
            var danhMuc = db.DanhMuc.Include(s=>s.DanhMuc1).Include(s=>s.DanhMuc2).Include(s=>s.SanPham).ToList();
            ViewBag.ID = id;
            return View(danhMuc);
        }
        public ActionResult TheoDanhMuc2(int? id)
        {
            var sanPham = db.SanPham.Include(s => s.DanhMuc).Include(s => s.KhuyenMai).Include(s => s.MauSac).Where(s => s.ID_DanhMuc == id).ToList();
            var danhMuc = db.DanhMuc.FirstOrDefault(s=>s.ID==id);
            ViewBag.TenDanhMuc = danhMuc.TenDanhMuc;
            return View(sanPham);
        }
        public ActionResult AddToCart(int sanPhamId, int soLuong)
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (Session["ID_KhachHang"] == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập hoặc đăng ký
                return RedirectToAction("Index", "Login", new { area = "Admin"}); // Thay đổi "Login" và "Account" thành tên action và controller tương ứng của trang đăng nhập
            }

            // Lấy ID của khách hàng từ Session
            int khachHangId = (int)Session["ID_KhachHang"];

            // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng của người dùng hay chưa
            var sanPhamTrongGioHang = db.GioHang.FirstOrDefault(g => g.ID_SanPham == sanPhamId && g.ID_KhachHang == khachHangId);
            var sanPham = db.SanPham.FirstOrDefault(g => g.ID == sanPhamId);
            if (sanPhamTrongGioHang != null)
            {
                if((sanPhamTrongGioHang.SoLuong + soLuong) > sanPham.SoLuong)
				{
                    TempData["Message"] = "Vượt quá số lượng đang có trong kho.";
                    return RedirectToAction("ChiTietSanPham", new { id = sanPhamId });
                }          
                else
				{
                    TempData["Message"] = "Sản phẩm đã có trong giỏ hàng.";
                    // Nếu sản phẩm đã tồn tại trong giỏ hàng, cập nhật số lượng
                    sanPhamTrongGioHang.SoLuong += soLuong;
                }                    
            }
            else
            {
                if(soLuong>sanPham.SoLuong)
				{
                    TempData["Message"] = "Vượt quá số lượng đang có trong kho.";
                    return RedirectToAction("ChiTietSanPham", new { id = sanPhamId });
                }  
                else
				{
                    TempData["Message"] = "Sản phẩm đã được thêm trong giỏ hàng.";
                    // Nếu sản phẩm chưa tồn tại trong giỏ hàng, thêm mới vào giỏ hàng
                    GioHang newItem = new GioHang
                    {
                        ID_SanPham = sanPhamId,
                        ID_KhachHang = khachHangId,
                        SoLuong = soLuong
                    };
                    db.GioHang.Add(newItem);
                }                      
            }

            db.SaveChanges();

            // Chuyển hướng về trang chi tiết sản phẩm sau khi thêm vào giỏ hàng
            return RedirectToAction("ChiTietSanPham", new { id = sanPhamId });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
       
    }
}
