using System;
using System.Collections.Generic;

namespace duan_totnghiep.Models;

public partial class Danhmuc
{
    public int Madm { get; set; }

    public string Tendm { get; set; } = null!;

    public string? Mota { get; set; }

    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
