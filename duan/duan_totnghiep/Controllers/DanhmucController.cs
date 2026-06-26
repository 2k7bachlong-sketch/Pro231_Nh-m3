using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace duan_totnghiep.Controllers
    {
        public class DanhmucController : Controller
        {
            private readonly AppDbContext _context;

            public DanhmucController(AppDbContext context)
            {
                _context = context;
            }

            // DANH SÁCH
            public async Task<IActionResult> Index()
            {
                var list = await _context.Danhmucs.ToListAsync();
                return View(list);
            }

            // THÊM - GET
            public IActionResult Them()
            {
                return View();
            }

            // THÊM - POST
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Them(Danhmuc dm)
            {
                if (ModelState.IsValid)
                {
                    _context.Danhmucs.Add(dm);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(dm);
            }

            // SỬA - GET
            public async Task<IActionResult> Sua(int id)
            {
                var dm = await _context.Danhmucs.FindAsync(id);
                if (dm == null) return NotFound();

                return View(dm);
            }

            // SỬA - POST
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Sua(Danhmuc dm)
            {
                if (ModelState.IsValid)
                {
                    _context.Danhmucs.Update(dm);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(dm);
            }

            // XÓA - GET
            public async Task<IActionResult> Xoa(int id)
            {
                var dm = await _context.Danhmucs.FindAsync(id);
                if (dm == null) return NotFound();

                return View(dm);
            }

            // XÓA - POST
            [HttpPost, ActionName("Xoa")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> XacNhanXoa(int id)
            {
                var dm = await _context.Danhmucs.FindAsync(id);
                if (dm != null)
                {
                    _context.Danhmucs.Remove(dm);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }

            // CHI TIẾT
            public async Task<IActionResult> Chitiet(int id)
            {
                var dm = await _context.Danhmucs
                    .Include(x => x.Sanphams)
                    .FirstOrDefaultAsync(x => x.Madm == id);

                if (dm == null) return NotFound();

                return View(dm);
            }
        }
    }


