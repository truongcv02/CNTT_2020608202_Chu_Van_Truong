namespace QLXe.Models
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HinhAnh")]
    public partial class HinhAnh
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Ảnh phía trước không được bỏ trống!")]
        [DisplayName("Ảnh Phía Trước")]
        [StringLength(255)]
        public string AnhTruoc { get; set; }
        [Required(ErrorMessage = "Ảnh bên trái không được bỏ trống!")]
        [DisplayName("Ảnh bên trái")]
        [StringLength(255)]
        public string AnhTrai { get; set; }
        [Required(ErrorMessage = "Ảnh phía sau không được bỏ trống!")]
        [DisplayName("Ảnh phía sau")]
        [StringLength(255)]
        public string AnhSau { get; set; }
        [Required(ErrorMessage = "Ảnh bên phải không được bỏ trống!")]
        [DisplayName("Ảnh bên phải")]
        [StringLength(255)]
        public string AnhPhai { get; set; }

        public int? ID_SanPham { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
