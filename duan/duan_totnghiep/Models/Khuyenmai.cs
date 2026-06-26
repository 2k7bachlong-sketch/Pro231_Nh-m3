using System;
using System.Collections.Generic;

namespace duan_totnghiep.Models;

public partial class Khuyenmai
{
    public int Makm { get; set; }

    public string Tenkm { get; set; } = null!;

    public int Phantramgiam { get; set; }

    public DateOnly? Ngaybatdau { get; set; }

    public DateOnly? Ngayketthuc { get; set; }

    public string? Trangthai { get; set; }

    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
