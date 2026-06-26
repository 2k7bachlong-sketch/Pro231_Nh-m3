using System;
using System.Collections.Generic;

namespace duan_totnghiep.Models;

public partial class Khachhang
{
    public int Makh { get; set; }

    public string Hoten { get; set; } = null!;

    public string? Sdt { get; set; }

    public string? Email { get; set; }

    public string? Diachi { get; set; }

    public int? Matk { get; set; }

    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();

    public virtual ICollection<Giohang> Giohangs { get; set; } = new List<Giohang>();

    public virtual Taikhoan? Taikhoan { get; set; }
}
