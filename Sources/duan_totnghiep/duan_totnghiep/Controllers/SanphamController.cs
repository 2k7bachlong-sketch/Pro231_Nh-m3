using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace duan_totnghiep.Controllers
{
    public class SanphamController : Controller
    {
        private readonly AppDbContext _context;

        public SanphamController(AppDbContext context)
        {
            _context = context;
        }

        // ================= DANH SÁCH =================
        public async Task<IActionResult> Index()
        {
            var ds = _context.Sanphams
                .Include(x => x.Danhmuc)
                .Include(x => x.Thuonghieu);

            return View(await ds.ToListAsync());
        }

        // ================= THÊM =================

        // GET
        public IActionResult Them()
        {
            ViewBag.Madm = new SelectList(_context.Danhmucs,
                                          "Madm", "Tendm");

            ViewBag.Math = new SelectList(_context.Thuonghieus,
                                          "Math", "Tenth");

            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(
            Sanpham sp,
            IFormFile fileAnh)
        {
            if (ModelState.IsValid)
            {
                // Upload ảnh
                if (fileAnh != null && fileAnh.Length > 0)
                {
                    string tenAnh = Guid.NewGuid().ToString()
                                      + Path.GetExtension(fileAnh.FileName);

                    string duongDan = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/images",
                        tenAnh);

                    using (var stream = new FileStream(duongDan, FileMode.Create))
                    {
                        await fileAnh.CopyToAsync(stream);
                    }

                    sp.Hinhanh = tenAnh;
                }

                _context.Sanphams.Add(sp);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Madm = new SelectList(_context.Danhmucs,
                                          "Madm",
                                          "Tendm",
                                          sp.Madm);

            ViewBag.Math = new SelectList(_context.Thuonghieus,
                                          "Math",
                                          "Tenth",
                                          sp.Math);

            return View(sp);
        }

        // ================= SỬA =================

        // GET
        public async Task<IActionResult> Sua(int id)
        {
            var sp = await _context.Sanphams.FindAsync(id);

            if (sp == null)
                return NotFound();

            ViewBag.Madm = new SelectList(_context.Danhmucs,
                                          "Madm",
                                          "Tendm",
                                          sp.Madm);

            ViewBag.Math = new SelectList(_context.Thuonghieus,
                                          "Math",
                                          "Tenth",
                                          sp.Math);

            return View(sp);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(
            int id,
            Sanpham sp,
            IFormFile? fileAnh)
        {
            if (id != sp.Masp)
                return NotFound();

            if (ModelState.IsValid)
            {
                // Nếu có chọn ảnh mới
                if (fileAnh != null && fileAnh.Length > 0)
                {
                    string tenAnh = Guid.NewGuid().ToString()
                                      + Path.GetExtension(fileAnh.FileName);

                    string duongDan = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/images",
                        tenAnh);

                    using (var stream = new FileStream(duongDan, FileMode.Create))
                    {
                        await fileAnh.CopyToAsync(stream);
                    }

                    sp.Hinhanh = tenAnh;
                }

                _context.Update(sp);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(sp);
        }

        // ================= XÓA =================

        // GET
        public async Task<IActionResult> Xoa(int id)
        {
            var sp = await _context.Sanphams
                .Include(x => x.Danhmuc)
                .Include(x => x.Thuonghieu)
                .FirstOrDefaultAsync(x => x.Masp == id);

            if (sp == null)
                return NotFound();

            return View(sp);
        }

        // POST
        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaConfirmed(int id)
        {
            var sp = await _context.Sanphams.FindAsync(id);

            if (sp != null)
            {
                _context.Sanphams.Remove(sp);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ================= CHI TIẾT =================

        public async Task<IActionResult> Chitiet(int id)
        {
            var sp = await _context.Sanphams
                .Include(x => x.Danhmuc)
                .Include(x => x.Thuonghieu)
                .FirstOrDefaultAsync(x => x.Masp == id);

            if (sp == null)
                return NotFound();

            return View(sp);
        }
    }
}