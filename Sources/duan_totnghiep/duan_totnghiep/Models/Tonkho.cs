using System;
using System.Collections.Generic;

namespace duan_totnghiep.Models;

public partial class Tonkho
{
    public int Matonkho { get; set; }

    public int Masp { get; set; }

    public int Soluongnhap { get; set; }

    public int? Soluongxuat { get; set; }

    public DateTime? Ngaycapnhat { get; set; }

    public virtual Sanpham Sanpham { get; set; } = null!;
}
