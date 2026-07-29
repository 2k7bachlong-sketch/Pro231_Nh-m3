using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.RegularExpressions;
namespace duan_totnghiep.Controllers
{
    public class ThanhtoanController : Controller
    {
        private readonly AppDbContext _context;

        public ThanhtoanController(AppDbContext context)
        {
            _context = context;
        }

        // Trang thanh toán
        public IActionResult Index()
        {
            int? maKh = HttpContext.Session.GetInt32("Makh");

            if (maKh == null)
                return RedirectToAction("Index", "Taikhoan");

            var gioHang = _context.Giohangs
                .Include(x => x.Sanpham)
                .ThenInclude(x => x.Khuyenmai)
                .Where(x => x.Makh == maKh)
                .ToList();

            var khach = _context.Khachhangs
                .FirstOrDefault(x => x.Makh == maKh);

            ViewBag.Khach = khach;

            ViewBag.TongTien = gioHang.Sum(x =>
            {
                decimal gia = x.Sanpham.Gia;

                if (x.Sanpham.Makm != null)
                {
                    var km = _context.Khuyenmais.FirstOrDefault(k => k.Makm == x.Sanpham.Makm);

                    if (km != null)
                    {
                        gia -= gia * km.Phantramgiam / 100m;
                    }
                }

                return gia * x.Soluong;
            });
            var voucher = _context.KhachhangKhuyenmais
            .Include(x => x.Khuyenmai)
            .Where(x => x.Makh == maKh &&
                        x.Dasudung == false)
            .ToList();

            ViewBag.Vouchers = voucher;
            return View(gioHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DatHang(string DiaChi,
                             string PhuongThuc,
                             int? makm)
        {
            int? maKh = HttpContext.Session.GetInt32("Makh");

            if (maKh == null)
                return RedirectToAction("Index", "Taikhoan");

            var gioHang = _context.Giohangs
                .Include(x => x.Sanpham)
                .Where(x => x.Makh == maKh)
                .ToList();

            if (!gioHang.Any())
            {
                TempData["Error"] = "Giỏ hàng trống!";
                return RedirectToAction("Index", "Giohang");
            }

            decimal tongTien = gioHang.Sum(x =>
            {
                decimal gia = x.Sanpham.Gia;

                if (x.Sanpham.Makm != null)
                {
                    var km = _context.Khuyenmais.FirstOrDefault(k => k.Makm == x.Sanpham.Makm);

                    if (km != null)
                    {
                        gia -= gia * km.Phantramgiam / 100m;
                    }
                }

                return gia * x.Soluong;
            });

            KhachhangKhuyenmai? voucherDaDung = null;

            if (makm != null)
            {
                voucherDaDung = _context.KhachhangKhuyenmais
                    .Include(x => x.Khuyenmai)
                  .FirstOrDefault(x =>
                        x.Makh == maKh.Value &&
                        x.Makm == makm.Value &&
                        x.Dasudung == false);

                if (voucherDaDung != null)
                {
                    tongTien -= tongTien *
                                voucherDaDung.Khuyenmai.Phantramgiam / 100m;

                    voucherDaDung.Dasudung = true;
                }
            }

            Donhang don = new Donhang
            {
                Makh = maKh.Value,
                Ngaydat = DateTime.Now,
                Tongtien = tongTien,
                Diachinhan = DiaChi,
                Phuongthucthanhtoan = PhuongThuc,
                Trangthai = PhuongThuc == "COD"
                                ? "Chờ xác nhận"
                                : "Chờ xác nhận thanh toán"
            };

            _context.Donhangs.Add(don);
            _context.SaveChanges();

            foreach (var item in gioHang)
            {
                decimal gia = item.Sanpham.Gia;

                // Nếu sản phẩm có khuyến mãi
                if (item.Sanpham.Makm != null)
                {
                    var km = _context.Khuyenmais
                        .FirstOrDefault(x => x.Makm == item.Sanpham.Makm);

                    if (km != null)
                    {
                        gia = gia - (gia * km.Phantramgiam / 100m);
                    }
                }

                Chitietdonhang ct = new Chitietdonhang
                {
                    Madh = don.Madh,
                    Masp = item.Masp,
                    Soluong = item.Soluong,
                    Dongia = gia,
                    Size = item.Size
                };

                _context.Chitietdonhangs.Add(ct);

                // Trừ tồn kho
                item.Sanpham.Soluongton -= item.Soluong;
            }

            _context.SaveChanges();
            
            // Xóa giỏ hàng
            _context.Giohangs.RemoveRange(gioHang);
            _context.SaveChanges();

            if (PhuongThuc == "QR")
            {
                return RedirectToAction("ThanhToanQR", new { id = don.Madh });
            }

            TempData["DatHangThanhCong"] = true;
            TempData["TrangThai"] = don.Trangthai;
            TempData["MaDon"] = don.Madh;
            TempData["NgayDat"] = don.Ngaydat?.ToString("dd/MM/yyyy HH:mm");
            TempData["TongTien"] = don.Tongtien.ToString("N0");

            return RedirectToAction("Index");
        }

        public IActionResult ThanhToanQR(int id)
        {
            var don = _context.Donhangs
                .FirstOrDefault(x => x.Madh == id);

            if (don == null)
                return RedirectToAction("Index");

            ViewBag.MaDon = don.Madh;
            ViewBag.SoTien = don.Tongtien;

            // Thay bằng tài khoản của bạn
            string nganHang = "MB";
            string stk = "0984968164";
            string tenTK = "Nguyễn Quốc Minh";

            ViewBag.QR =
                $"https://img.vietqr.io/image/{nganHang}-{stk}-compact2.png?amount={don.Tongtien}&addInfo=DH{don.Madh}&accountName={Uri.EscapeDataString(tenTK)}";

            return View(don);
        }

        [HttpPost]
        public IActionResult Webhook([FromBody] SepayWebhook data)
        {
            try
            {
                if (data == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Không có dữ liệu"
                    });
                }

                // Ví dụ nội dung chuyển khoản: DH15
                var match = Regex.Match(data.content ?? "", @"DH(\d+)");

                // Không đúng cú pháp => vẫn trả về success để SePay không gửi lại
                if (!match.Success)
                {
                    return Ok(new
                    {
                        success = true
                    });
                }

                int maDon = int.Parse(match.Groups[1].Value);

                var don = _context.Donhangs.FirstOrDefault(x => x.Madh == maDon);

                // Không tìm thấy đơn hàng
                if (don == null)
                {
                    return Ok(new
                    {
                        success = true
                    });
                }

                // Kiểm tra số tiền
                if (don.Tongtien != data.transferAmount)
                {
                    return Ok(new
                    {
                        success = true
                    });
                }

                // Nếu đơn chưa được cập nhật thì cập nhật
                if (don.Trangthai != "Chờ xác nhận")
                {
                    don.Trangthai = "Chờ xác nhận";
                    _context.SaveChanges();
                }

                return Ok(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        [HttpGet]
        public IActionResult KiemTraThanhToan(int id)
        {
            var don = _context.Donhangs.FirstOrDefault(x => x.Madh == id);

            if (don == null)
                return Json(new { daThanhToan = false });

            return Json(new
            {
                daThanhToan = don.Trangthai == "Chờ xác nhận"
            });
        }
    }
}
