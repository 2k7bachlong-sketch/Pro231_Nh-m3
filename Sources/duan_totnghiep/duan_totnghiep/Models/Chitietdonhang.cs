using System;
using System.Collections.Generic;

namespace duan_totnghiep.Models;

public partial class Chitietdonhang
{
    public int Madh { get; set; }

    public int Masp { get; set; }

    public int Soluong { get; set; }

    public decimal Dongia { get; set; }

    public virtual Donhang Donhang { get; set; } = null!;

    public virtual Sanpham Sanpham { get; set; } = null!;
}
