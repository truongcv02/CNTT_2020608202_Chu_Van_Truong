using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLXe.Models;
using PagedList;
namespace QLXe.Areas.Admin.Controllers
{
    public class DonHangsController : Controller
    {
        private QLXeDB db = new QLXeDB();

        // GET: Admin/DonHangs
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            var donHang = db.DonHang.Include(d => d.KhachHang);
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            ViewBag.currentFilter = SearchString;
            if (SearchString != null)
            {
                donHang = donHang.Where(s => s.KhachHang.Ten.Contains(SearchString));
            }
            donHang = donHang.OrderBy(s => s.TinhTrang);
            int PageSize = 5;
            int PageNumber = (page ?? 1);
            return View(donHang.ToPagedList(PageNumber, PageSize));
        }

        // GET: Admin/DonHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHang.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // GET: Admin/DonHangs/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: Admin/DonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NgayDat,NgayGiaoHang,DiaChiGiaoHang,TongGiaTri,ID_KhachHang,TinhTrang,SDT")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.DonHang.Add(donHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

          
            return View(donHang);
        }

        // GET: Admin/DonHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHang.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }
        // POST: Admin/DonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection form)
        {
            int ID = int.Parse(form["ID"]);
            DateTime NgayGiaoHang = DateTime.Parse(form["NgayGiaoHang"]);
            int TinhTrang = int.Parse(form["TinhTrang"]);
            
            DonHang f = db.DonHang.FirstOrDefault(s => s.ID == ID);
            if (ModelState.IsValid)
            {
                f.NgayGiaoHang = NgayGiaoHang;
                KhachHang khachHang = db.KhachHang.FirstOrDefault(s => s.ID == f.ID_KhachHang);
                if(TinhTrang!=f.TinhTrang && TinhTrang ==2)
				{
                    string strSanPham = "";
                    var ctdh = f.ChiTietDonHang.ToList();
                    foreach (var item in ctdh)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>" + item.SanPham.TenXe + "</td>";
                        strSanPham += "<td>" + item.SoLuong.ToString() + "</td>";
                        strSanPham += "<td>" + (item.SoLuong * item.SanPham.GiaBan * ( 100- item.SanPham.KhuyenMai.PhanTramGiamGia)/100).ToString() + "</td>";
                        strSanPham += "</tr>";

                    }
                    
                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/send2.html"));
                    contentCustomer = contentCustomer.Replace("{{Header}}", "Đơn hàng của bạn đã được xác nhận <br/> Đơn hàng như sau");
                    contentCustomer = contentCustomer.Replace("{{MaDon}}", f.ID.ToString());
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", f.NgayDat.ToString());
                    contentCustomer = contentCustomer.Replace("{{TenKhach}}", khachHang.Ho + " " + khachHang.Ten);
                    contentCustomer = contentCustomer.Replace("{{SDT}}", f.SDT);
                    contentCustomer = contentCustomer.Replace("{{TinhTrang}}", "Đã xác nhận");
                    contentCustomer = contentCustomer.Replace("{{Mail}}", khachHang.Email);
                    contentCustomer = contentCustomer.Replace("{{DiaChi}}", f.DiaChiGiaoHang);
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", f.TongGiaTri.ToString());
                    QLXe.Common.common.SendMail("ShopOnline", "Đơn Hàng #" + f.ID, contentCustomer, khachHang.Email);
                    string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/send1.html"));
                    contentAdmin = contentAdmin.Replace("{{Header}}", "Bạn vừa xác nhận đơn hàng <br/> Đơn hàng như sau");
                    contentAdmin = contentAdmin.Replace("{{MaDon}}", f.ID.ToString());
                    contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                    contentAdmin = contentAdmin.Replace("{{NgayDat}}", f.NgayDat.ToString());
                    contentAdmin = contentAdmin.Replace("{{TenKhach}}", khachHang.Ho + " " + khachHang.Ten);
                    contentAdmin = contentAdmin.Replace("{{SDT}}", f.SDT);
                    contentAdmin = contentAdmin.Replace("{{TinhTrang}}", "Đã xác nhận");
                    contentAdmin = contentAdmin.Replace("{{Mail}}", khachHang.Email);
                    contentAdmin = contentAdmin.Replace("{{DiaChi}}", f.DiaChiGiaoHang);
                    contentAdmin = contentAdmin.Replace("{{TongTien}}", f.TongGiaTri.ToString());
                    QLXe.Common.common.SendMail("ShopOnline", "Đơn Hàng #" + f.ID, contentAdmin, "motorcycle1234321@gmail.com");
                }                    
                f.TinhTrang = TinhTrang;
                db.Entry(f).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(f);
        }
        // GET: Admin/DonHangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHang.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // POST: Admin/DonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DonHang donHang = db.DonHang.Find(id);
            db.DonHang.Remove(donHang);
            db.SaveChanges();
            return RedirectToAction("Index");
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
