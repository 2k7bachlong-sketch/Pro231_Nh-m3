using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace duan_totnghiep.Controllers
{
    public class NhanvienController : Controller
    {
        private readonly AppDbContext _context;

        public NhanvienController(AppDbContext context)
        {
            _context = context;
        }

        //================ DANH SÁCH =================

        public async Task<IActionResult> Index()
        {
            var ds = _context.Nhanviens
                .Include(x => x.Taikhoan);

            return View(await ds.ToListAsync());
        }

        //================ THÊM =================

        public IActionResult Them()
        {
            ViewBag.Matk = new SelectList(_context.Taikhoans, "Matk", "Tendangnhap");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(Nhanvien nv)
        {
            if (ModelState.IsValid)
            {
                _context.Nhanviens.Add(nv);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Matk = new SelectList(_context.Taikhoans, "Matk", "Tendangnhap", nv.Matk);

            return View(nv);
        }

        //================ SỬA =================

        public async Task<IActionResult> Sua(int id)
        {
            var nv = await _context.Nhanviens.FindAsync(id);

            if (nv == null)
                return NotFound();

            ViewBag.Matk = new SelectList(_context.Taikhoans, "Matk", "Tendangnhap", nv.Matk);

            return View(nv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(int id, Nhanvien nv)
        {
            if (id != nv.Manv)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(nv);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Matk = new SelectList(_context.Taikhoans, "Matk", "Tendangnhap", nv.Matk);

            return View(nv);
        }

        //================ XÓA =================

        public async Task<IActionResult> Xoa(int id)
        {
            var nv = await _context.Nhanviens
                .Include(x => x.Taikhoan)
                .FirstOrDefaultAsync(x => x.Manv == id);

            if (nv == null)
                return NotFound();

            return View(nv);
        }

        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaConfirmed(int id)
        {
            var nv = await _context.Nhanviens.FindAsync(id);

            if (nv != null)
            {
                _context.Nhanviens.Remove(nv);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        //================ CHI TIẾT =================

        public async Task<IActionResult> Chitiet(int id)
        {
            var nv = await _context.Nhanviens
                .Include(x => x.Taikhoan)
                .FirstOrDefaultAsync(x => x.Manv == id);

            if (nv == null)
                return NotFound();

            return View(nv);
        }
    }
}