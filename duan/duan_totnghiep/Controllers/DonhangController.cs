using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace duan_totnghiep.Controllers
{
    public class DonhangController : Controller
    {
        private readonly AppDbContext _context;

        public DonhangController(AppDbContext context)
        {
            _context = context;
        }

        // Danh sách
        public async Task<IActionResult> Index()
        {
            var ds = _context.Donhangs
                .Include(x => x.Khachhang)
                .Include(x => x.Nhanvien);

            // Đếm đơn hàng mới
            ViewBag.SoDonMoi = await _context.Donhangs
                .CountAsync(x => x.Trangthai == "Chờ xác nhận");

            return View(await ds.ToListAsync());
        }

        // Chi tiết
        public async Task<IActionResult> ChiTiet(int id)
        {
            var dh = await _context.Donhangs
                .Include(x => x.Khachhang)
                .Include(x => x.Nhanvien)
                .FirstOrDefaultAsync(x => x.Madh == id);

            if (dh == null)
                return NotFound();

            return View(dh);
        }

        // GET Thêm
        public IActionResult Lap()
        {
 
            ViewBag.KhachHang = new SelectList(
                _context.Khachhangs,
                "Makh",
                "Hoten"
            );

            ViewBag.NhanVien = new SelectList(
                _context.Nhanviens,
                "Manv",
                "Hoten"
            );

            return View();
        }

        // POST Thêm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lap(Donhang dh)
        {
            // Bỏ validate Navigation Property
            ModelState.Remove("Khachhang");
            ModelState.Remove("Nhanvien");

            if (ModelState.IsValid)
            {
                _context.Donhangs.Add(dh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

                ViewBag.KhachHang = new SelectList(
                _context.Khachhangs,
                "Makh",
                "Hoten"
                );

                ViewBag.NhanVien = new SelectList(
                    _context.Nhanviens,
                    "Manv",
                    "Hoten"
                );

            return View(dh);
        }

        // GET Sửa
        public async Task<IActionResult> Sua(int id)
        {
            var dh = await _context.Donhangs.FindAsync(id);

            if (dh == null)
                return NotFound();

            ViewBag.NhanVien = new SelectList(
                _context.Nhanviens,
                "Manv",
                "Hoten",
                dh.Manv);

            return View(dh);
        }

        // POST Sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(int id, Donhang dh)
        {
            if (id != dh.Madh)
                return NotFound();

            var don = await _context.Donhangs.FindAsync(id);

            if (don == null)
                return NotFound();

            // Chỉ cập nhật những trường được phép
            don.Manv = dh.Manv;          

            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã cập nhật đơn hàng.";

            return RedirectToAction(nameof(Index));
        }

        //Tính doanh thu 
        [HttpPost]
        public async Task<IActionResult> CapNhatTrangThai(int id, string trangThai)
        {
            var don = await _context.Donhangs.FindAsync(id);

            if (don == null)
                return NotFound();

            // Không cho sửa nếu đơn đã kết thúc
            if (don.Trangthai == "Đã hoàn thành" ||
                don.Trangthai == "Đã hủy")
            {
                TempData["Error"] = "Đơn hàng đã kết thúc.";
                return RedirectToAction(nameof(Index));
            }

            don.Trangthai = trangThai;

            await _context.SaveChangesAsync();

            switch (trangThai)
            {
                case "Đã xác nhận":
                    TempData["Success"] = $"Đơn #{don.Madh} đã được xác nhận.";
                    break;

                case "Đang giao":
                    TempData["Success"] = $"Đơn #{don.Madh} đã bàn giao cho vận chuyển.";
                    break;

                case "Đã giao":
                    TempData["Success"] = $"Đơn #{don.Madh} đã giao thành công, chờ Admin xác nhận.";
                    break;

                case "Đã hoàn thành":
                    TempData["Success"] = $"Đơn #{don.Madh} đã hoàn thành.";
                    break;

                case "Đã hủy":
                    TempData["Success"] = $"Đơn #{don.Madh} đã bị hủy.";
                    break;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET Xóa
        public async Task<IActionResult> Xoa(int id)
        {
            var dh = await _context.Donhangs
                .Include(x => x.Khachhang)
                .Include(x => x.Nhanvien)
                .FirstOrDefaultAsync(x => x.Madh == id);

            if (dh == null)
                return NotFound();

            if (dh.Trangthai == "Đã hoàn thành")
            {
                TempData["Error"] = "Không thể xóa đơn hàng đã hoàn thành.";
                return RedirectToAction(nameof(Index));
            }

            if (dh.Trangthai == "Đang giao")
            {
                TempData["Error"] = "Không thể xóa đơn hàng đang giao.";
                return RedirectToAction(nameof(Index));
            }

            if (dh.Trangthai == "Đã hủy")
            {
                TempData["Error"] = "Không thể xóa đơn hàng đã hủy.";
                return RedirectToAction(nameof(Index));
            }

            return View(dh);
        }

        // POST Xóa
        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaXacNhan(int id)
        {
            var dh = await _context.Donhangs.FindAsync(id);

            if (dh == null)
                return RedirectToAction(nameof(Index));

            // Xóa tất cả chi tiết đơn hàng trước
            var chiTiet = _context.Chitietdonhangs
                                  .Where(x => x.Madh == id)
                                  .ToList();

            _context.Chitietdonhangs.RemoveRange(chiTiet);

            // Sau đó mới xóa đơn hàng
            _context.Donhangs.Remove(dh);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã xóa đơn hàng thành công.";

            return RedirectToAction(nameof(Index));
        }

        // ================= ĐƠN HÀNG CỦA KHÁCH =================

        public async Task<IActionResult> Cuatoi(string trangThai = "TatCa")
        {
            int? maKh = HttpContext.Session.GetInt32("Makh");

            if (maKh == null)
                return RedirectToAction("Index", "Taikhoan");

            var ds = _context.Donhangs
                .Include(x => x.Chitietdonhangs)
                .ThenInclude(x => x.Sanpham)
                .ThenInclude(x => x.Thuonghieu)
                .Where(x => x.Makh == maKh);

            if (trangThai != "TatCa")
            {
                ds = ds.Where(x => x.Trangthai == trangThai);
            }

            ViewBag.TrangThai = trangThai;

            return View(await ds
                .OrderByDescending(x => x.Ngaydat)
                .ToListAsync());
        }


        // Khách huỷ đơn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HuyDon(int id)
        {
            int? maKh = HttpContext.Session.GetInt32("Makh");

            if (maKh == null)
                return RedirectToAction("Index", "Taikhoan");


            var don = await _context.Donhangs
                .FirstOrDefaultAsync(x => x.Madh == id && x.Makh == maKh);


            if (don == null)
                return NotFound();


            // Chỉ cho huỷ khi chưa xác nhận
            if (don.Trangthai == "Chờ xác nhận")
            {
                don.Trangthai = "Đã hủy";

                await _context.SaveChangesAsync();
            }


            return RedirectToAction("Cuatoi");
        }
    }
}