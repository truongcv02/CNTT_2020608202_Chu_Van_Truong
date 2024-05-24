namespace QLXe.Models
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietDonHang")]
    public partial class ChiTietDonHang
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Số lượng không được bỏ trống!")]
        [DisplayName("Số lượng")]
        public int? SoLuong { get; set; }

        public int? ID_DonHang { get; set; }

        public int? ID_SanPham { get; set; }

        public virtual DonHang DonHang { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
