namespace QLXe.Models
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KhuyenMai")]
    public partial class KhuyenMai
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhuyenMai()
        {
            SanPham = new HashSet<SanPham>();
        }

        public int ID { get; set; }
        [Required(ErrorMessage = "Tên khuyến mãi không được bỏ trống!")]
        [DisplayName("Tên khuyến mãi")]
        [StringLength(100)]
        public string TenKhuyenMai { get; set; }
        [Required(ErrorMessage = "Phần trăm giảm giá không được bỏ trống!")]
        [DisplayName("Số phần trăm giảm giá")]
        public int? PhanTramGiamGia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPham> SanPham { get; set; }
    }
}
