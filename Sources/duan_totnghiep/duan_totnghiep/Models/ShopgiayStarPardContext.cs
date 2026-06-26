using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace duan_totnghiep.Models;

public partial class ShopgiayStarPardContext : DbContext
{
    public ShopgiayStarPardContext()
    {
    }

    public ShopgiayStarPardContext(DbContextOptions<ShopgiayStarPardContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chitietdonhang> Chitietdonhangs { get; set; }

    public virtual DbSet<Danhmuc> Danhmucs { get; set; }

    public virtual DbSet<Donhang> Donhangs { get; set; }

    public virtual DbSet<Giohang> Giohangs { get; set; }

    public virtual DbSet<Khachhang> Khachhangs { get; set; }

    public virtual DbSet<Khuyenmai> Khuyenmais { get; set; }

    public virtual DbSet<Nhanvien> Nhanviens { get; set; }

    public virtual DbSet<Sanpham> Sanphams { get; set; }

    public virtual DbSet<Taikhoan> Taikhoans { get; set; }

    public virtual DbSet<Thuonghieu> Thuonghieus { get; set; }

    public virtual DbSet<Tonkho> Tonkhos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-9JAC447\\SQLEXPRESS02;Database=SHOPGIAY_StarPard;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chitietdonhang>(entity =>
        {
            entity.HasKey(e => new { e.Madh, e.Masp }).HasName("PK__CHITIETD__563D28E49DEDBC22");

            entity.ToTable("CHITIETDONHANG");

            entity.Property(e => e.Madh).HasColumnName("MADH");
            entity.Property(e => e.Masp).HasColumnName("MASP");
            entity.Property(e => e.Dongia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("DONGIA");
            entity.Property(e => e.Soluong).HasColumnName("SOLUONG");

            entity.HasOne(d => d.Donhang).WithMany(p => p.Chitietdonhangs)
                .HasForeignKey(d => d.Madh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETDON__MADH__59063A47");

            entity.HasOne(d => d.Sanpham).WithMany(p => p.Chitietdonhangs)
                .HasForeignKey(d => d.Masp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETDON__MASP__59FA5E80");
        });

        modelBuilder.Entity<Danhmuc>(entity =>
        {
            entity.HasKey(e => e.Madm).HasName("PK__DANHMUC__603F005C0AEF4A9E");

            entity.ToTable("DANHMUC");

            entity.Property(e => e.Madm).HasColumnName("MADM");
            entity.Property(e => e.Mota)
                .HasMaxLength(255)
                .HasColumnName("MOTA");
            entity.Property(e => e.Tendm)
                .HasMaxLength(100)
                .HasColumnName("TENDM");
        });

        modelBuilder.Entity<Donhang>(entity =>
        {
            entity.HasKey(e => e.Madh).HasName("PK__DONHANG__603F00474E0B6F13");

            entity.ToTable("DONHANG");

            entity.Property(e => e.Madh).HasColumnName("MADH");
            entity.Property(e => e.Diachinhan)
                .HasMaxLength(255)
                .HasColumnName("DIACHINHAN");
            entity.Property(e => e.Makh).HasColumnName("MAKH");
            entity.Property(e => e.Manv).HasColumnName("MANV");
            entity.Property(e => e.Ngaydat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NGAYDAT");
            entity.Property(e => e.Phuongthucthanhtoan)
                .HasMaxLength(50)
                .HasColumnName("PHUONGTHUCTHANHTOAN");
            entity.Property(e => e.Tongtien)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TONGTIEN");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(50)
                .HasDefaultValue("Chờ xác nhận")
                .HasColumnName("TRANGTHAI");

            entity.HasOne(d => d.Khachhang).WithMany(p => p.Donhangs)
                .HasForeignKey(d => d.Makh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DONHANG__MAKH__5535A963");

            entity.HasOne(d => d.Nhanvien).WithMany(p => p.Donhangs)
                .HasForeignKey(d => d.Manv)
                .HasConstraintName("FK__DONHANG__MANV__5629CD9C");
        });

        modelBuilder.Entity<Giohang>(entity =>
        {
            entity.HasKey(e => e.Magh).HasName("PK__GIOHANG__603F38A359157EC1");

            entity.ToTable("GIOHANG");

            entity.HasIndex(e => new { e.Makh, e.Masp }, "UQ_GIOHANG").IsUnique();

            entity.Property(e => e.Magh).HasColumnName("MAGH");
            entity.Property(e => e.Makh).HasColumnName("MAKH");
            entity.Property(e => e.Masp).HasColumnName("MASP");
            entity.Property(e => e.Ngaytao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NGAYTAO");
            entity.Property(e => e.Soluong)
                .HasDefaultValue(1)
                .HasColumnName("SOLUONG");

            entity.HasOne(d => d.Khachhang).WithMany(p => p.Giohangs)
                .HasForeignKey(d => d.Makh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GIOHANG__MAKH__4F7CD00D");

            entity.HasOne(d => d.Sanpham).WithMany(p => p.Giohangs)
                .HasForeignKey(d => d.Masp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GIOHANG__MASP__5070F446");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.Makh).HasName("PK__KHACHHAN__603F592C4F735A0F");

            entity.ToTable("KHACHHANG");

            entity.Property(e => e.Makh).HasColumnName("MAKH");
            entity.Property(e => e.Diachi)
                .HasMaxLength(255)
                .HasColumnName("DIACHI");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Hoten)
                .HasMaxLength(100)
                .HasColumnName("HOTEN");
            entity.Property(e => e.Matk).HasColumnName("MATK");
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("SDT");

            entity.HasOne(d => d.Taikhoan).WithMany(p => p.Khachhangs)
                .HasForeignKey(d => d.Matk)
                .HasConstraintName("FK__KHACHHANG__MATK__3A81B327");
        });

        modelBuilder.Entity<Khuyenmai>(entity =>
        {
            entity.HasKey(e => e.Makm).HasName("PK__KHUYENMA__603F592B89E4EFD3");

            entity.ToTable("KHUYENMAI");

            entity.Property(e => e.Makm).HasColumnName("MAKM");
            entity.Property(e => e.Ngaybatdau).HasColumnName("NGAYBATDAU");
            entity.Property(e => e.Ngayketthuc).HasColumnName("NGAYKETTHUC");
            entity.Property(e => e.Phantramgiam).HasColumnName("PHANTRAMGIAM");
            entity.Property(e => e.Tenkm)
                .HasMaxLength(100)
                .HasColumnName("TENKM");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(50)
                .HasColumnName("TRANGTHAI");
        });

        modelBuilder.Entity<Nhanvien>(entity =>
        {
            entity.HasKey(e => e.Manv).HasName("PK__NHANVIEN__603F511415C5751B");

            entity.ToTable("NHANVIEN");

            entity.Property(e => e.Manv).HasColumnName("MANV");
            entity.Property(e => e.Chucvu)
                .HasMaxLength(50)
                .HasColumnName("CHUCVU");
            entity.Property(e => e.Diachi)
                .HasMaxLength(255)
                .HasColumnName("DIACHI");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Hoten)
                .HasMaxLength(100)
                .HasColumnName("HOTEN");
            entity.Property(e => e.Matk).HasColumnName("MATK");
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("SDT");

            entity.HasOne(d => d.Taikhoan).WithMany(p => p.Nhanviens)
                .HasForeignKey(d => d.Matk)
                .HasConstraintName("FK__NHANVIEN__MATK__3D5E1FD2");
        });

        modelBuilder.Entity<Sanpham>(entity =>
        {
            entity.HasKey(e => e.Masp).HasName("PK__SANPHAM__60228A321B28625E");

            entity.ToTable("SANPHAM");

            entity.Property(e => e.Masp).HasColumnName("MASP");
            entity.Property(e => e.Gia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("GIA");
            entity.Property(e => e.Hinhanh)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("HINHANH");
            entity.Property(e => e.Madm).HasColumnName("MADM");
            entity.Property(e => e.Makm).HasColumnName("MAKM");
            entity.Property(e => e.Math).HasColumnName("MATH");
            entity.Property(e => e.Mota)
                .HasMaxLength(500)
                .HasColumnName("MOTA");
            entity.Property(e => e.Ngaytao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NGAYTAO");
            entity.Property(e => e.Soluongton)
                .HasDefaultValue(0)
                .HasColumnName("SOLUONGTON");
            entity.Property(e => e.Tensp)
                .HasMaxLength(100)
                .HasColumnName("TENSP");

            entity.HasOne(d => d.Danhmuc).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.Madm)
                .HasConstraintName("FK__SANPHAM__MADM__48CFD27E");

            entity.HasOne(d => d.Khuyenmai).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.Makm)
                .HasConstraintName("FK__SANPHAM__MAKM__49C3F6B7");

            entity.HasOne(d => d.Thuonghieu).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.Math)
                .HasConstraintName("FK__SANPHAM__MATH__47DBAE45");
        });

        modelBuilder.Entity<Taikhoan>(entity =>
        {
            entity.HasKey(e => e.Matk).HasName("PK__TAIKHOAN__602372161A000639");

            entity.ToTable("TAIKHOAN");

            entity.HasIndex(e => e.Tendangnhap, "UQ__TAIKHOAN__6C836FE55E543377").IsUnique();

            entity.Property(e => e.Matk).HasColumnName("MATK");
            entity.Property(e => e.Matkhau)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("MATKHAU");
            entity.Property(e => e.Tendangnhap)
                .HasMaxLength(50)
                .HasColumnName("TENDANGNHAP");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(30)
                .HasColumnName("TRANGTHAI");
            entity.Property(e => e.Vaitro)
                .HasMaxLength(30)
                .HasColumnName("VAITRO");
        });

        modelBuilder.Entity<Thuonghieu>(entity =>
        {
            entity.HasKey(e => e.Math).HasName("PK__THUONGHI__6023721BEA10CE92");

            entity.ToTable("THUONGHIEU");

            entity.Property(e => e.Math).HasColumnName("MATH");
            entity.Property(e => e.Mota)
                .HasMaxLength(255)
                .HasColumnName("MOTA");
            entity.Property(e => e.Tenth)
                .HasMaxLength(100)
                .HasColumnName("TENTH");
        });

        modelBuilder.Entity<Tonkho>(entity =>
        {
            entity.HasKey(e => e.Matonkho).HasName("PK__TONKHO__9E1E742172BA90AD");

            entity.ToTable("TONKHO");

            entity.Property(e => e.Matonkho).HasColumnName("MATONKHO");
            entity.Property(e => e.Masp).HasColumnName("MASP");
            entity.Property(e => e.Ngaycapnhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NGAYCAPNHAT");
            entity.Property(e => e.Soluongnhap).HasColumnName("SOLUONGNHAP");
            entity.Property(e => e.Soluongxuat)
                .HasDefaultValue(0)
                .HasColumnName("SOLUONGXUAT");

            entity.HasOne(d => d.Sanpham).WithMany(p => p.Tonkhos)
                .HasForeignKey(d => d.Masp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TONKHO__MASP__5EBF139D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
