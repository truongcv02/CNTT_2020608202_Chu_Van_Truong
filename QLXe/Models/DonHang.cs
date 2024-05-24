namespace QLXe.Models
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DonHang")]
    public partial class DonHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonHang()
        {
            ChiTietDonHang = new HashSet<ChiTietDonHang>();
        }

        public int ID { get; set; }
        [Required(ErrorMessage = "Ngày đặt không được bỏ trống!")]
        [DisplayName("Ngày đặt")]
        [Column(TypeName = "date")]
        public DateTime? NgayDat { get; set; }
        [Required(ErrorMessage = "Ngày giao hàng không được bỏ trống!")]
        [DisplayName("Ngày giao hàng")]
        [Column(TypeName = "date")]
        public DateTime? NgayGiaoHang { get; set; }
        [Required(ErrorMessage = "Địa chỉ giao hàng không được bỏ trống!")]
        [DisplayName("Địa chỉ giao hàng")]
        [StringLength(100)]
        public string DiaChiGiaoHang { get; set; }
        [Required(ErrorMessage = "Tổng giá trị không được bỏ trống!")]
        [DisplayName("Tổng giá trị")]
        public decimal? TongGiaTri { get; set; }

        public int? ID_KhachHang { get; set; }
        [Required(ErrorMessage = "Tình trạng không được bỏ trống!")]
        [DisplayName("Tình trạng")]
        public int? TinhTrang { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được bỏ trống!")]
        [DisplayName("Số điện thoại")]
        [StringLength(15)]
        public string SDT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHang { get; set; }
        public virtual KhachHang KhachHang { get; set; }
    }
}
