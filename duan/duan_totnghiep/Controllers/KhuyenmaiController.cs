using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace duan_totnghiep.Controllers
{
    public class KhuyenmaiController : Controller
    {
        private readonly AppDbContext _context;

        public KhuyenmaiController(AppDbContext context)
        {
            _context = context;
        }

        // Danh sách
        public async Task<IActionResult> Index()
        {
            return View(await _context.Khuyenmais.ToListAsync());
        }

        // Chi tiết
        public async Task<IActionResult> ChiTiet(int id)
        {
            var km = await _context.Khuyenmais
                .FirstOrDefaultAsync(x => x.Makm == id);

            if (km == null)
                return NotFound();

            return View(km);
        }

        // GET Thêm
        public IActionResult Them()
        {
            return View();
        }

        // POST Thêm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(Khuyenmai km)
        {
            ModelState.Remove("Sanphams");

            if (ModelState.IsValid)
            {
                _context.Khuyenmais.Add(km);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(km);
        }

        // GET Sửa
        public async Task<IActionResult> Sua(int id)
        {
            var km = await _context.Khuyenmais.FindAsync(id);

            if (km == null)
                return NotFound();

            return View(km);
        }

        // POST Sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(int id, Khuyenmai km)
        {
            if (id != km.Makm)
                return NotFound();

            ModelState.Remove("Sanphams");

            if (ModelState.IsValid)
            {
                _context.Update(km);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(km);
        }

        // GET Xóa
        public async Task<IActionResult> Xoa(int id)
        {
            var km = await _context.Khuyenmais
                .FirstOrDefaultAsync(x => x.Makm == id);

            if (km == null)
                return NotFound();

            return View(km);
        }

        // POST Xóa
        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaXacNhan(int id)
        {
            var km = await _context.Khuyenmais.FindAsync(id);

            if (km != null)
            {
                _context.Khuyenmais.Remove(km);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}