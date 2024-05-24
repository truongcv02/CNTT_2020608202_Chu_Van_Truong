using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLXe.Models;
using System.Net.Mail;
using System.Net;
using QLXe.Areas.Admin.Momo;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace QLXe.Controllers
{
    public class DatHangController : Controller
    {
        
        public ActionResult NhapThongTin()
		{
            return View();
		}
        // GET: DatHang
        public ActionResult Index(string ids, string DiaChi, string DienThoai)
        {
            try
            {
                if (!string.IsNullOrEmpty(ids))
                {
                    // Kiểm tra xem ID Khách hàng từ Session có tồn tại và có thể chuyển đổi thành số nguyên hay không
                    if (Session["ID_KhachHang"] != null && int.TryParse(Session["ID_KhachHang"].ToString(), out int id))
                    {
                        QLXeDB db = new QLXeDB();
                        var donHang = new DonHang();

                        // Lấy thông tin Khách hàng từ cơ sở dữ liệu
                        KhachHang khachHang = db.KhachHang.FirstOrDefault(s => s.ID == id);
                        // Thiết lập thông tin đơn hàng
                        donHang.ID_KhachHang = id;
                        donHang.NgayDat = DateTime.Now;
                        donHang.NgayGiaoHang = DateTime.Now;
                        donHang.SDT = DienThoai;
                        donHang.TinhTrang = 1;
                        donHang.DiaChiGiaoHang = DiaChi;
                        donHang.TongGiaTri = 0; // Khởi tạo tổng giá trị đơn hàng

                        // Tách các ID sản phẩm thành một mảng và tính tổng giá trị của đơn hàng
                        var idSanPhamArray = ids.Split(',');
                        foreach (var item in idSanPhamArray)
                        {
                            if (int.TryParse(item, out int sanPhamId) && sanPhamId != 0)
                            {
                                // Lấy thông tin sản phẩm từ cơ sở dữ liệu
                                SanPham sp = db.SanPham.FirstOrDefault(s => s.ID == sanPhamId);
                                // Lấy thông tin giỏ hàng từ cơ sở dữ liệu
                                GioHang gioHang = db.GioHang.FirstOrDefault(s => s.ID_SanPham == sanPhamId && s.ID_KhachHang == id);
                                if (sp != null && gioHang != null)
                                {

                                    // Cộng dồn giá của từng sản phẩm vào tổng giá trị của đơn hàng
                                    donHang.TongGiaTri += sp.GiaBan * gioHang.SoLuong * (100 - sp.KhuyenMai.PhanTramGiamGia)/100;
                                }
                            }
                        }

                        // Lưu đơn hàng vào cơ sở dữ liệu
                        db.DonHang.Add(donHang);
                        db.SaveChanges();

                        // Lấy ID của đơn hàng vừa được lưu
                        int donHangId = donHang.ID;
                        // Lưu chi tiết đơn hàng
                        foreach (var item in idSanPhamArray)
                        {
                            int a = int.Parse(item);
                            if (a != 0)
                            {
                                SanPham sp = db.SanPham.FirstOrDefault(s => s.ID == a);
                                GioHang gioHang = db.GioHang.FirstOrDefault(s => s.ID_SanPham == sp.ID && s.ID_KhachHang == id);
                                var chiTietDonHang = new ChiTietDonHang();

                                chiTietDonHang.ID_DonHang = donHangId; // Sử dụng ID của đơn hàng vừa lưu
                                chiTietDonHang.ID_SanPham = sp.ID;
                                chiTietDonHang.SoLuong = gioHang.SoLuong;

                                db.ChiTietDonHang.Add(chiTietDonHang);
                            }
                        }
                        foreach (var item in idSanPhamArray)
                        {
                            if (int.TryParse(item, out int sanPhamId) && sanPhamId != 0)
                            {
                                
                                GioHang gioHang = db.GioHang.FirstOrDefault(s => s.ID_SanPham == sanPhamId && s.ID_KhachHang == id);
                                var sanPham = db.SanPham.Find(sanPhamId).SoLuong -= gioHang.SoLuong;
                                db.GioHang.Remove(gioHang);
                            }
                        }
                        // Lưu thay đổi vào cơ sở dữ liệu
                        db.SaveChanges();
                        ViewBag.result = true;
						//sendmail

                        var strSanPham = "";
                        foreach(var item in idSanPhamArray)
						{
                            int a = int.Parse(item);
                            if (a != 0)
                            {
                                SanPham sp = db.SanPham.FirstOrDefault(s => s.ID == a);
                                GioHang gioHang = db.GioHang.FirstOrDefault(s => s.ID_SanPham == sp.ID && s.ID_KhachHang == id);
                                ChiTietDonHang chiTietDonHang = db.ChiTietDonHang.FirstOrDefault(s => s.ID_DonHang == donHang.ID && s.ID_SanPham == sp.ID);
                                strSanPham += "<tr>";
                                strSanPham += "<td>"+sp.TenXe+"</td>";
                                strSanPham += "<td>" + chiTietDonHang.SoLuong.ToString() + "</td>";
                                strSanPham += "<td>" + (chiTietDonHang.SoLuong*sp.GiaBan*(100-sp.KhuyenMai.PhanTramGiamGia)/100).ToString()+"</td>";
                                strSanPham += "</tr>";
                            }
                        }
                        string contentCustomer = System.IO.File.ReadAllText( Server.MapPath("~/Content/template/send2.html"));
                        contentCustomer = contentCustomer.Replace("{{Header}}", "Cảm ơn đã đặt hàng. Đơn hàng sẽ bị tạm giữ cho đến khi chúng tôi xác nhận thanh toán hoàn thành.Trong thời gian chờ đợi, đây là lời nhắc về những gì bạn đã đặt hàng: ");
                        contentCustomer = contentCustomer.Replace("{{MaDon}}", donHang.ID.ToString());
                        contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                        contentCustomer = contentCustomer.Replace("{{NgayDat}}", donHang.NgayDat.ToString());
                        contentCustomer = contentCustomer.Replace("{{TenKhach}}", khachHang.Ho + " " + khachHang.Ten);
                        contentCustomer = contentCustomer.Replace("{{SDT}}", donHang.SDT);
                        contentCustomer = contentCustomer.Replace("{{TinhTrang}}", "Chờ xác nhận");
                        contentCustomer = contentCustomer.Replace("{{Mail}}", khachHang.Email);
                        contentCustomer = contentCustomer.Replace("{{DiaChi}}", donHang.DiaChiGiaoHang);
                        contentCustomer = contentCustomer.Replace("{{TongTien}}", donHang.TongGiaTri.ToString());
                        QLXe.Common.common.SendMail("ShopOnline", "Đơn Hàng #" + donHang.ID, contentCustomer, khachHang.Email);
                        string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/send1.html"));
                        contentAdmin = contentAdmin.Replace("{{Header}}", "Bạn vừa nhận được đơn hàng từ");
                        contentAdmin = contentAdmin.Replace("{{MaDon}}", donHang.ID.ToString());
                        contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                        contentAdmin = contentAdmin.Replace("{{NgayDat}}", donHang.NgayDat.ToString());
                        contentAdmin = contentAdmin.Replace("{{TenKhach}}", khachHang.Ho + " " + khachHang.Ten);
                        contentAdmin = contentAdmin.Replace("{{SDT}}", donHang.SDT);
                        contentAdmin = contentAdmin.Replace("{{TinhTrang}}", "Chờ xác nhận");
                        contentAdmin = contentAdmin.Replace("{{Mail}}", khachHang.Email);
                        contentAdmin = contentAdmin.Replace("{{DiaChi}}", donHang.DiaChiGiaoHang);
                        contentAdmin = contentAdmin.Replace("{{TongTien}}", donHang.TongGiaTri.ToString());
                        QLXe.Common.common.SendMail("ShopOnline", "Đơn Hàng #" + donHang.ID, contentAdmin, "motorcycle1234321@gmail.com");
                        int idss = id;
                        return RedirectToAction("Index", "DonHangs", new { id = idss, tt = 1});
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.result = false;
            }
            return View();
        }
        public string ThanhToanMomo(int? idDonHang, float sotien)
        {
            string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
            string partnerCode = "MOMO5RGX20191128";
            string accessKey = "M8brj9K6E22vXoDB";
            string serectkey = "nqQiVSgDMy809JoPF6OzP5OdBUB550Y4";
            string orderInfo = "Store HD";
            string redirectUrl = "https://localhost:44304/DatHang/XacNhan";
            string ipnUrl = "https://localhost:44304/Huy";
            string requestType = "captureWallet";
            TempData["idDonHang"] = idDonHang;
            string amount = sotien.ToString();
            string orderId = Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";
            //Before sign HMAC SHA256 signature
            string rawHash = "accessKey=" + accessKey +
                "&amount=" + amount +
                "&extraData=" + extraData +
                "&ipnUrl=" + ipnUrl +
                "&orderId=" + orderId +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + partnerCode +
                "&redirectUrl=" + redirectUrl +
                "&requestId=" + requestId +
                "&requestType=" + requestType
                ;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "partnerName", "Test" },
                { "storeId", "MomoTestStore" },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "lang", "en" },
                { "extraData", extraData },
                { "requestType", requestType },
                { "signature", signature }

            };
            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
            Console.WriteLine(responseFromMomo);
            JObject jmessage = JObject.Parse(responseFromMomo);

            return jmessage.GetValue("payUrl").ToString();
        }
        public ActionResult Index2(string ids, string DiaChi, string DienThoai)
        {
            QLXeDB db = new QLXeDB();
            try
            {
                if (!string.IsNullOrEmpty(ids))
                {
                    // Kiểm tra xem ID Khách hàng từ Session có tồn tại và có thể chuyển đổi thành số nguyên hay không
                    if (Session["ID_KhachHang"] != null && int.TryParse(Session["ID_KhachHang"].ToString(), out int id))
                    {
                        var donHang = new DonHang();

                        // Lấy thông tin Khách hàng từ cơ sở dữ liệu
                        KhachHang khachHang = db.KhachHang.FirstOrDefault(s => s.ID == id);
                        // Thiết lập thông tin đơn hàng
                        donHang.ID_KhachHang = id;
                        donHang.NgayDat = DateTime.Now;
                        donHang.NgayGiaoHang = DateTime.Now;
                        donHang.SDT = DienThoai;
                        donHang.TinhTrang = 1;
                        donHang.DiaChiGiaoHang = DiaChi;
                        donHang.TongGiaTri = 0; // Khởi tạo tổng giá trị đơn hàng

                        // Tách các ID sản phẩm thành một mảng và tính tổng giá trị của đơn hàng
                        var idSanPhamArray = ids.Split(',');
                        foreach (var item in idSanPhamArray)
                        {
                            if (int.TryParse(item, out int sanPhamId) && sanPhamId != 0)
                            {
                                // Lấy thông tin sản phẩm từ cơ sở dữ liệu
                                SanPham sp = db.SanPham.FirstOrDefault(s => s.ID == sanPhamId);
                                // Lấy thông tin giỏ hàng từ cơ sở dữ liệu
                                GioHang gioHang = db.GioHang.FirstOrDefault(s => s.ID_SanPham == sanPhamId && s.ID_KhachHang == id);
                                if (sp != null && gioHang != null)
                                {

                                    // Cộng dồn giá của từng sản phẩm vào tổng giá trị của đơn hàng
                                    donHang.TongGiaTri += sp.GiaBan * gioHang.SoLuong * (100 - sp.KhuyenMai.PhanTramGiamGia) / 100;
                                }
                            }
                        }

                        // Lưu đơn hàng vào cơ sở dữ liệu
                        db.DonHang.Add(donHang);
                        db.SaveChanges();

                        // Lấy ID của đơn hàng vừa được lưu
                        int donHangId = donHang.ID;
                        // Lưu chi tiết đơn hàng
                        foreach (var item in idSanPhamArray)
                        {
                            int a = int.Parse(item);
                            if (a != 0)
                            {
                                SanPham sp = db.SanPham.FirstOrDefault(s => s.ID == a);
                                GioHang gioHang = db.GioHang.FirstOrDefault(s => s.ID_SanPham == sp.ID && s.ID_KhachHang == id);
                                var chiTietDonHang = new ChiTietDonHang();
                                chiTietDonHang.ID_DonHang = donHangId; // Sử dụng ID của đơn hàng vừa lưu
                                chiTietDonHang.ID_SanPham = sp.ID;
                                chiTietDonHang.SoLuong = gioHang.SoLuong;
                                db.ChiTietDonHang.Add(chiTietDonHang);
                            }
                        }
                        foreach (var item in idSanPhamArray)
                        {
                            if (int.TryParse(item, out int sanPhamId) && sanPhamId != 0)
                            {

                                GioHang gioHang = db.GioHang.FirstOrDefault(s => s.ID_SanPham == sanPhamId && s.ID_KhachHang == id);
                                var sanPham = db.SanPham.Find(sanPhamId).SoLuong -= gioHang.SoLuong;
                                db.GioHang.Remove(gioHang);
                            }
                        }
                        // Lưu thay đổi vào cơ sở dữ liệu
                        db.SaveChanges();
                        ViewBag.result = true;
                        //sendmail

                        var strSanPham = "";
                        foreach (var item in idSanPhamArray)
                        {
                            int a = int.Parse(item);
                            if (a != 0)
                            {
                                SanPham sp = db.SanPham.FirstOrDefault(s => s.ID == a);
                                GioHang gioHang = db.GioHang.FirstOrDefault(s => s.ID_SanPham == sp.ID && s.ID_KhachHang == id);
                                ChiTietDonHang chiTietDonHang = db.ChiTietDonHang.FirstOrDefault(s => s.ID_DonHang == donHang.ID && s.ID_SanPham == sp.ID);
                                strSanPham += "<tr>";
                                strSanPham += "<td>" + sp.TenXe + "</td>";
                                strSanPham += "<td>" + chiTietDonHang.SoLuong.ToString() + "</td>";
                                strSanPham += "<td>" + (chiTietDonHang.SoLuong * sp.GiaBan * (100 - sp.KhuyenMai.PhanTramGiamGia) / 100).ToString() + "</td>";
                                strSanPham += "</tr>";
                            }
                        }
                        string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/send2.html"));
                        contentCustomer = contentCustomer.Replace("{{Header}}", "Cảm ơn đã đặt hàng. Đơn hàng sẽ bị tạm giữ cho đến khi chúng tôi xác nhận thanh toán hoàn thành.Trong thời gian chờ đợi, đây là lời nhắc về những gì bạn đã đặt hàng: ");
                        contentCustomer = contentCustomer.Replace("{{MaDon}}", donHang.ID.ToString());
                        contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                        contentCustomer = contentCustomer.Replace("{{NgayDat}}", donHang.NgayDat.ToString());
                        contentCustomer = contentCustomer.Replace("{{TenKhach}}", khachHang.Ho + " " + khachHang.Ten);
                        contentCustomer = contentCustomer.Replace("{{SDT}}", donHang.SDT);
                        contentCustomer = contentCustomer.Replace("{{TinhTrang}}", "Chờ xác nhận");
                        contentCustomer = contentCustomer.Replace("{{Mail}}", khachHang.Email);
                        contentCustomer = contentCustomer.Replace("{{DiaChi}}", donHang.DiaChiGiaoHang);
                        contentCustomer = contentCustomer.Replace("{{TongTien}}", donHang.TongGiaTri.ToString());
                        QLXe.Common.common.SendMail("ShopOnline", "Đơn Hàng #" + donHang.ID, contentCustomer, khachHang.Email);
                        string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/send1.html"));
                        contentAdmin = contentAdmin.Replace("{{Header}}", "Bạn vừa nhận được đơn hàng từ");
                        contentAdmin = contentAdmin.Replace("{{MaDon}}", donHang.ID.ToString());
                        contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                        contentAdmin = contentAdmin.Replace("{{NgayDat}}", donHang.NgayDat.ToString());
                        contentAdmin = contentAdmin.Replace("{{TenKhach}}", khachHang.Ho + " " + khachHang.Ten);
                        contentAdmin = contentAdmin.Replace("{{SDT}}", donHang.SDT);
                        contentAdmin = contentAdmin.Replace("{{TinhTrang}}", "Chờ xác nhận");
                        contentAdmin = contentAdmin.Replace("{{Mail}}", khachHang.Email);
                        contentAdmin = contentAdmin.Replace("{{DiaChi}}", donHang.DiaChiGiaoHang);
                        contentAdmin = contentAdmin.Replace("{{TongTien}}", donHang.TongGiaTri.ToString());
                        QLXe.Common.common.SendMail("ShopOnline", "Đơn Hàng #" + donHang.ID, contentAdmin, "motorcycle1234321@gmail.com");
                        var link = ThanhToanMomo(donHang.ID, (float)500000);
                        return Redirect(link);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.result = false;
            }
            return View();
        }
        public ActionResult ThanhToanLai(int? idDonHang)
		{
            var link = ThanhToanMomo(idDonHang, (float)500000);
            return Redirect(link);
        }
        public async Task<ActionResult> XacNhan(string orderId, string requestId)
        {
            QLXeDB db = new QLXeDB();
            int id = int.Parse(TempData["idDonHang"].ToString());
            var donHang = db.DonHang.FirstOrDefault(o => o.ID == id);
            int khachHangID = (int)donHang.ID_KhachHang;
            string partnerCode = "MOMO5RGX20191128";
            string serectkey = "nqQiVSgDMy809JoPF6OzP5OdBUB550Y4";
            string accessKey = "M8brj9K6E22vXoDB";
            string endpoint = "https://test-payment.momo.vn//v2/gateway/api/query";
            //Before sign HMAC SHA256 signature
            string rawHash = "accessKey=" + accessKey +
                "&orderId=" + orderId +
                "&partnerCode=" + partnerCode +
                "&requestId=" + requestId
                ;
            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "requestId", requestId },
                { "orderId", orderId },
                { "signature", signature },
                { "lang", "en" }

            };
            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
            JObject jmessage = JObject.Parse(responseFromMomo);
            string result = jmessage.GetValue("resultCode").ToString();
            if (result == "0")
            {
                //xử lý thanh toán thành công
                ViewBag.Message = "Thanh toán đơn hàng thành công ";
                ViewBag.Madonhang = orderId;

                if (donHang != null)
                {
                    donHang.TinhTrang = 6;
                    db.SaveChanges();
                }
            }
            else
            {
                donHang.TinhTrang = 5;
                if (donHang.TinhTrang == 5)
                {
                    db.SaveChanges();
                    ViewBag.Message = "Thanh toán đơn hàng thất bại ";
                    ViewBag.Madonhang = orderId;
                    
                    return RedirectToAction("Index", "DonHangs", new { id = khachHangID, tt = 5 });
                }
            }
            return RedirectToAction("Index","DonHangs",new { id = khachHangID, tt = 6 });
        }
    }
}