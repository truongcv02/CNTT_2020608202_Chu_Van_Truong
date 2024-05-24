namespace QLXe.Models
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KhachHang")]
    public partial class KhachHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhachHang()
        {
            DonHang = new HashSet<DonHang>();
            GioHang = new HashSet<GioHang>();
            TaiKhoan = new HashSet<TaiKhoan>();
        }

        public int ID { get; set; }
        [Required(ErrorMessage = "Tên khách hàng không được bỏ trống!")]
        [DisplayName("Tên khách hàng")]
        [StringLength(50)]
        public string Ten { get; set; }
        [Required(ErrorMessage = "Họ khách hàng không được bỏ trống!")]
        [DisplayName("Họ khách hàng")]
        [StringLength(50)]
        public string Ho { get; set; }

        [Required(ErrorMessage = "Email không được bỏ trống!")]
        [DisplayName("Địa chỉ Email")]

        [StringLength(100)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được bỏ trống!")]
        [DisplayName("Số điện thoại")]
        [StringLength(20)]
        public string DienThoai { get; set; }
        [Required(ErrorMessage = "Địa chỉ không được bỏ trống!")]
        [DisplayName("Địa chỉ")]
        [StringLength(20)]
        public string DiaChi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHang> DonHang { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GioHang> GioHang { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaiKhoan> TaiKhoan { get; set; }
    }
}
