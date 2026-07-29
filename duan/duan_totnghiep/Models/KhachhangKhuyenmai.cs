using System;
using System.Collections.Generic;

namespace duan_totnghiep.Models;

public partial class KhachhangKhuyenmai
{
    public int Id { get; set; }

    public int Makh { get; set; }

    public int Makm { get; set; }

    public bool? Dasudung { get; set; }

    public DateTime? Ngaynhan { get; set; }

    public virtual Khachhang Khachhang { get; set; } = null!;

    public virtual Khuyenmai Khuyenmai { get; set; } = null!;
}
