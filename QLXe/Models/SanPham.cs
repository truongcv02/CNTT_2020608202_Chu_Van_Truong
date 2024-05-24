namespace QLXe.Models
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            ChiTietDonHang = new HashSet<ChiTietDonHang>();
            DonNhapHang = new HashSet<DonNhapHang>();
            GioHang = new HashSet<GioHang>();
            HinhAnh = new HashSet<HinhAnh>();
            Slide = new HashSet<Slide>();
        }

        public int ID { get; set; }
        [Required(ErrorMessage = "Tên xe không được bỏ trống!")]
        [DisplayName("Tên xe")]
        [StringLength(100)]
        public string TenXe { get; set; }
        [Required(ErrorMessage = "Năm sản xuất không được bỏ trống!")]
        [DisplayName("Năm sản xuất")]
        public int? NamSX { get; set; }
        [Required(ErrorMessage = "Thông số không được bỏ trống!")]
        [DisplayName("Thông số kỹ thuật")]
        [Column(TypeName = "text")]
        public string ThongSo { get; set; }
        [Required(ErrorMessage = "Giá Nhập không được bỏ trống!")]
        [DisplayName("Giá nhập")]
        public decimal? GiaNhap { get; set; }
        [Required(ErrorMessage = "Giá bán không được bỏ trống!")]
        [DisplayName("Giá bán")]
        public decimal? GiaBan { get; set; }

        public int? ID_DanhMuc { get; set; }

        public int? ID_KhuyenMai { get; set; }
        [Required(ErrorMessage = "Số lượng không được bỏ trống!")]
        [DisplayName("Số lượng")]
        public int? SoLuong { get; set; }

        public int? ID_MauSac { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHang { get; set; }

        public virtual DanhMuc DanhMuc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonNhapHang> DonNhapHang { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GioHang> GioHang { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HinhAnh> HinhAnh { get; set; }

        public virtual KhuyenMai KhuyenMai { get; set; }

        public virtual MauSac MauSac { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Slide> Slide { get; set; }
    }
}
