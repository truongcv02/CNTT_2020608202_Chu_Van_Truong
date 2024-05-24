namespace QLXe.Models
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GioHang")]
    public partial class GioHang
    {
        public int ID { get; set; }

        public int? ID_SanPham { get; set; }

        public int? ID_KhachHang { get; set; }
        [Required(ErrorMessage = "Số lượng không được bỏ trống!")]
        [DisplayName("Số lượng")]
        public int? SoLuong { get; set; }

        public virtual KhachHang KhachHang { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
