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

            // Không cho sửa nếu đã kết thúc
            if (don.Trangthai == "Đã hoàn thành" ||
                don.Trangthai == "Đã hủy")
            {
                return RedirectToAction(nameof(Index));
            }

            don.Trangthai = trangThai;

            await _context.SaveChangesAsync();

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

            if (dh.Trangthai != "Đang xử lý")
            {
                TempData["Error"] = "Chỉ có thể xóa đơn đang xử lý.";
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

            if (dh != null)
            {
                _context.Donhangs.Remove(dh);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}