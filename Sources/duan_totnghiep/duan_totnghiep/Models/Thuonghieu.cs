using System;
using System.Collections.Generic;

namespace duan_totnghiep.Models;

public partial class Thuonghieu
{
    public int Math { get; set; }

    public string Tenth { get; set; } = null!;

    public string? Mota { get; set; }

    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
