using System;
using System.Collections.Generic;

namespace duan_totnghiep.Models;

public partial class Sanpham
{
    public int Masp { get; set; }

    public string Tensp { get; set; } = null!;

    public decimal Gia { get; set; }

    public string? Mota { get; set; }

    public string? Hinhanh { get; set; }

    public int? Soluongton { get; set; }

    public DateTime? Ngaytao { get; set; }

    public int? Math { get; set; }

    public int? Madm { get; set; }

    public int? Makm { get; set; }

    public virtual ICollection<Chitietdonhang> Chitietdonhangs { get; set; } = new List<Chitietdonhang>();

    public virtual ICollection<Giohang> Giohangs { get; set; } = new List<Giohang>();

    public virtual Danhmuc? Danhmuc { get; set; }

    public virtual Khuyenmai? Khuyenmai { get; set; }

    public virtual Thuonghieu? Thuonghieu { get; set; }

    public virtual ICollection<Tonkho> Tonkhos { get; set; } = new List<Tonkho>();
}
