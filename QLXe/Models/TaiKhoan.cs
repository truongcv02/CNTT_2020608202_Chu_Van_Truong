namespace QLXe.Models
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaiKhoan")]
    public partial class TaiKhoan
    {
        QLXeDB db = new QLXeDB();
        public int ID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Tên đăng nhập không được bỏ trống!")]
        [DisplayName("Tên đăng nhập")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống!")]
        [DisplayName("Mật khẩu")]
        [StringLength(100)]
        public string Password { get; set; }

        public int? ID_KhachHang { get; set; }
        [Required(ErrorMessage = "Khóa không được bỏ trống!")]
        [DisplayName("Khóa")]
        public bool Khoa { get; set; }

        public bool? Thay(int id)
        {
            var user = db.TaiKhoan.Find(id);
            user.Khoa = !user.Khoa;
            return !user.Khoa;
        }
		[Column(TypeName = "date")]
        [DisplayName("Ngày lập")]
        public DateTime? NgayLap { get; set; }
        public virtual KhachHang KhachHang { get; set; }
    }
}
