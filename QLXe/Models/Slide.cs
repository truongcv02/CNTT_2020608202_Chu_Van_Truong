namespace QLXe.Models
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Slide")]
    public partial class Slide
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Hình ảnh không được bỏ trống!")]
        [DisplayName("Hình ảnh")]
        [Column("Slide")]
        [StringLength(255)]
        public string Slide1 { get; set; }

        public int? ID_SanPham { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
