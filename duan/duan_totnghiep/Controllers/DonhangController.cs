using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Them()
        {
            ViewBag.KhachHang = _context.Khachhangs.ToList();
            ViewBag.NhanVien = _context.Nhanviens.ToList();

            return View();
        }

        // POST Thêm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(Donhang dh)
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

            ViewBag.KhachHang = _context.Khachhangs.ToList();
            ViewBag.NhanVien = _context.Nhanviens.ToList();

            return View(dh);
        }

        // GET Sửa
        public async Task<IActionResult> Sua(int id)
        {
            var dh = await _context.Donhangs.FindAsync(id);

            if (dh == null)
                return NotFound();

            ViewBag.KhachHang = _context.Khachhangs.ToList();
            ViewBag.NhanVien = _context.Nhanviens.ToList();

            return View(dh);
        }

        // POST Sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(int id, Donhang dh)
        {
            if (id != dh.Madh)
                return NotFound();

            // Bỏ validate Navigation Property
            ModelState.Remove("Khachhang");
            ModelState.Remove("Nhanvien");

            if (ModelState.IsValid)
            {
                _context.Update(dh);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.KhachHang = _context.Khachhangs.ToList();
            ViewBag.NhanVien = _context.Nhanviens.ToList();

            return View(dh);
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