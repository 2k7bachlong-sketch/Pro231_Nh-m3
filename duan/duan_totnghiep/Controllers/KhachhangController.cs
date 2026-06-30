using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace duan_totnghiep.Controllers
{
    public class KhachhangController : Controller
    {
        private readonly AppDbContext _context;

        public KhachhangController(AppDbContext context)
        {
            _context = context;
        }

        // Danh sách
        public async Task<IActionResult> Index()
        {
            return View(await _context.Khachhangs.ToListAsync());
        }

        // Chi tiết
        public async Task<IActionResult> ChiTiet(int id)
        {
            var kh = await _context.Khachhangs
                .FirstOrDefaultAsync(x => x.Makh == id);

            if (kh == null)
                return NotFound();

            return View(kh);
        }

        // GET Thêm
        public IActionResult Them()
        {
            return View();
        }

        // POST Thêm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(Khachhang kh)
        {
            if (ModelState.IsValid)
            {
                _context.Khachhangs.Add(kh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(kh);
        }

        // GET Sửa
        public async Task<IActionResult> Sua(int id)
        {
            var kh = await _context.Khachhangs.FindAsync(id);

            if (kh == null)
                return NotFound();

            return View(kh);
        }

        // POST Sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(int id, Khachhang kh)
        {
            if (id != kh.Makh)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(kh);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(kh);
        }

        // GET Xóa
        public async Task<IActionResult> Xoa(int id)
        {
            var kh = await _context.Khachhangs
                .FirstOrDefaultAsync(x => x.Makh == id);

            if (kh == null)
                return NotFound();

            return View(kh);
        }

        // POST Xóa
        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaXacNhan(int id)
        {
            var kh = await _context.Khachhangs.FindAsync(id);

            if (kh != null)
            {
                _context.Khachhangs.Remove(kh);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}