using System;
using System.Collections.Generic;

namespace duan_totnghiep.Models;

public partial class Taikhoan
{
    public int Matk { get; set; }

    public string Tendangnhap { get; set; } = null!;

    public string Matkhau { get; set; } = null!;

    public string Vaitro { get; set; } = null!;

    public string Trangthai { get; set; } = null!;

    public virtual ICollection<Khachhang> Khachhangs { get; set; } = new List<Khachhang>();

    public virtual ICollection<Nhanvien> Nhanviens { get; set; } = new List<Nhanvien>();
}
