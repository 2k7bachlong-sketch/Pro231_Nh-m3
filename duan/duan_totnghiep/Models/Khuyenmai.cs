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

    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();

    public virtual ICollection<KhachhangKhuyenmai> KhachhangKhuyenmais { get; set; } = new List<KhachhangKhuyenmai>();

    //Hàm tính Ngày 
    public string TrangThaiHienThi
    {
        get
        {
            if (!Ngaybatdau.HasValue || !Ngayketthuc.HasValue)
                return "Không xác định";

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            if (today < Ngaybatdau.Value)
                return "Sắp diễn ra";

            if (today <= Ngayketthuc.Value)
                return "Hoạt động";

            return "Đã kết thúc";
        }
    }
    public string ThongTinTrangThai
    {
        get
        {
            if (!Ngaybatdau.HasValue || !Ngayketthuc.HasValue)
                return "";

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            // Chưa bắt đầu
            if (today < Ngaybatdau.Value)
            {
                int days = Ngaybatdau.Value.DayNumber - today.DayNumber;

                if (days == 0)
                    return "Bắt đầu hôm nay";

                if (days == 1)
                    return "Bắt đầu sau 1 ngày";

                return $"Bắt đầu sau {days} ngày";
            }

            // Đang hoạt động
            if (today <= Ngayketthuc.Value)
            {
                int days = Ngayketthuc.Value.DayNumber - today.DayNumber;

                if (days == 0)
                    return "Kết thúc hôm nay";

                if (days == 1)
                    return "Còn 1 ngày";

                return $"Còn {days} ngày";
            }

            // Đã kết thúc
            int passed = today.DayNumber - Ngayketthuc.Value.DayNumber;

            if (passed == 1)
                return "Đã kết thúc 1 ngày trước";

            return $"Đã kết thúc {passed} ngày trước";
        }
    }
    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
