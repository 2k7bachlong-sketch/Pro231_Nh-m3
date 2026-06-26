using System;
using System.Collections.Generic;

namespace duan_totnghiep.Models;

public partial class Donhang
{
    public int Madh { get; set; }

    public int Makh { get; set; }

    public int? Manv { get; set; }

    public DateTime? Ngaydat { get; set; }

    public decimal Tongtien { get; set; }

    public string? Trangthai { get; set; }

    public string? Diachinhan { get; set; }

    public string? Phuongthucthanhtoan { get; set; }

    public virtual ICollection<Chitietdonhang> Chitietdonhangs { get; set; } = new List<Chitietdonhang>();

    public virtual Khachhang Khachhang { get; set; } = null!;

    public virtual Nhanvien? Nhanvien { get; set; }
}
