using System;
using System.Collections.Generic;

namespace duan_totnghiep.Models;

public partial class Giohang
{
    public int Magh { get; set; }

    public int? Makh { get; set; }

    public DateTime? Ngaytao { get; set; }

    public int? Masp { get; set; }

    public int? Soluong { get; set; }

    public virtual Khachhang? Khachhang{ get; set; }

    public virtual Sanpham? Sanpham { get; set; }
}
