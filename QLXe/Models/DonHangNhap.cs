namespace QLXe.Models
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DonNhapHang")]
    public partial class DonNhapHang
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Số lượng không được bỏ trống!")]
        [DisplayName("Số lượng")]
        public int SoLuong { get; set; }
        [Required(ErrorMessage = "Ngày nhập không được bỏ trống!")]
        [DisplayName("Ngày nhập")]
        [Column(TypeName = "date")]
        public DateTime NgayNhap { get; set; }

        public int ID_SanPham { get; set; }

        public int ID_NhaSanXuat { get; set; }

        public virtual NhaSanXuat NhaSanXuat { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
