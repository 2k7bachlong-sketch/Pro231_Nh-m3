using System;
using System.Collections.Generic;

namespace duan_totnghiep.Models;

public partial class Thongke
{
    public int Matkke { get; set; }

    public DateOnly? Ngaythongke { get; set; }

    public decimal? Doanhthu { get; set; }

    public int? Soluongdonhang { get; set; }

    public int? Soluongkhachhang { get; set; }

    public int? Soluongsanphamban { get; set; }

    public int? Manv { get; set; }

    public virtual Nhanvien? Nhanvien { get; set; }
}
