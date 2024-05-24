namespace QLXe.Models
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NhaSanXuat")]
    public partial class NhaSanXuat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhaSanXuat()
        {
             DonNhapHang = new HashSet<DonNhapHang>();
        }

        public int ID { get; set; }
        [Required(ErrorMessage = "Tên hãng không được bỏ trống!")]
        [DisplayName("Tên hãng")]
        [StringLength(50)]
        public string TenHang { get; set; }
        [Required(ErrorMessage = "Sô điện thoại không được bỏ trống!")]
        [DisplayName("Số điện thoại")]
        [StringLength(15)]
        public string SoDienThoai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonNhapHang> DonNhapHang { get; set; }
    }
}
