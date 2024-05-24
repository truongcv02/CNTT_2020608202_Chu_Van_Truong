using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace QLXe.Models
{
	public partial class QLXeDB : DbContext
	{
		public QLXeDB()
			: base("name=QLXeDB")
		{
		}

		public virtual DbSet<ChiTietDonHang> ChiTietDonHang { get; set; }
		public virtual DbSet<DanhMuc> DanhMuc { get; set; }
		public virtual DbSet<DonHang> DonHang { get; set; }
		public virtual DbSet<DonNhapHang> DonNhapHang { get; set; }
		public virtual DbSet<GioHang> GioHang { get; set; }
		public virtual DbSet<HinhAnh> HinhAnh { get; set; }
		public virtual DbSet<KhachHang> KhachHang { get; set; }
		public virtual DbSet<KhuyenMai> KhuyenMai { get; set; }
		public virtual DbSet<MauSac> MauSac { get; set; }
		public virtual DbSet<NhaSanXuat> NhaSanXuat { get; set; }
		public virtual DbSet<SanPham> SanPham { get; set; }
		public virtual DbSet<Slide> Slide { get; set; }
		public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
		public virtual DbSet<TaiKhoan> TaiKhoan { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<DanhMuc>()
				.HasMany(e => e.DanhMuc1)
				.WithOptional(e => e.DanhMuc2)
				.HasForeignKey(e => e.ID_DanhMuc);

			modelBuilder.Entity<DanhMuc>()
				.HasMany(e => e.SanPham)
				.WithOptional(e => e.DanhMuc)
				.HasForeignKey(e => e.ID_DanhMuc);

			modelBuilder.Entity<DonHang>()
				.Property(e => e.TongGiaTri)
				.HasPrecision(15, 2);

			modelBuilder.Entity<DonHang>()
				.Property(e => e.SDT)
				.IsUnicode(false);

			modelBuilder.Entity<DonHang>()
				.HasMany(e => e.ChiTietDonHang)
				.WithOptional(e => e.DonHang)
				.HasForeignKey(e => e.ID_DonHang);

			modelBuilder.Entity<HinhAnh>()
				.Property(e => e.AnhTruoc)
				.IsUnicode(false);

			modelBuilder.Entity<HinhAnh>()
				.Property(e => e.AnhTrai)
				.IsUnicode(false);

			modelBuilder.Entity<HinhAnh>()
				.Property(e => e.AnhSau)
				.IsUnicode(false);

			modelBuilder.Entity<HinhAnh>()
				.Property(e => e.AnhPhai)
				.IsUnicode(false);

			modelBuilder.Entity<KhachHang>()
				.HasMany(e => e.DonHang)
				.WithOptional(e => e.KhachHang)
				.HasForeignKey(e => e.ID_KhachHang);

			modelBuilder.Entity<KhachHang>()
				.HasMany(e => e.GioHang)
				.WithOptional(e => e.KhachHang)
				.HasForeignKey(e => e.ID_KhachHang);

			modelBuilder.Entity<KhachHang>()
				.HasMany(e => e.TaiKhoan)
				.WithOptional(e => e.KhachHang)
				.HasForeignKey(e => e.ID_KhachHang);

			modelBuilder.Entity<KhuyenMai>()
				.HasMany(e => e.SanPham)
				.WithOptional(e => e.KhuyenMai)
				.HasForeignKey(e => e.ID_KhuyenMai);

			modelBuilder.Entity<MauSac>()
				.HasMany(e => e.SanPham)
				.WithOptional(e => e.MauSac)
				.HasForeignKey(e => e.ID_MauSac);

			modelBuilder.Entity<NhaSanXuat>()
				.Property(e => e.SoDienThoai)
				.IsUnicode(false);

			modelBuilder.Entity<NhaSanXuat>()
				.HasMany(e => e.DonNhapHang)
				.WithRequired(e => e.NhaSanXuat)
				.HasForeignKey(e => e.ID_NhaSanXuat)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<SanPham>()
				.Property(e => e.ThongSo)
				.IsUnicode(false);

			modelBuilder.Entity<SanPham>()
				.Property(e => e.GiaBan)
				.HasPrecision(15, 2);

			modelBuilder.Entity<SanPham>()
				.Property(e => e.GiaNhap)
				.HasPrecision(15, 2);

			modelBuilder.Entity<SanPham>()
				.HasMany(e => e.ChiTietDonHang)
				.WithOptional(e => e.SanPham)
				.HasForeignKey(e => e.ID_SanPham);

			modelBuilder.Entity<SanPham>()
				.HasMany(e => e.DonNhapHang)
				.WithRequired(e => e.SanPham)
				.HasForeignKey(e => e.ID_SanPham)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<SanPham>()
				.HasMany(e => e.GioHang)
				.WithOptional(e => e.SanPham)
				.HasForeignKey(e => e.ID_SanPham);

			modelBuilder.Entity<SanPham>()
				.HasMany(e => e.HinhAnh)
				.WithOptional(e => e.SanPham)
				.HasForeignKey(e => e.ID_SanPham);

			modelBuilder.Entity<SanPham>()
				.HasMany(e => e.Slide)
				.WithOptional(e => e.SanPham)
				.HasForeignKey(e => e.ID_SanPham);

			modelBuilder.Entity<Slide>()
				.Property(e => e.Slide1)
				.IsUnicode(false);
		}
	}
}
