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
            var ds = await _context.Nhanviens
                .Include(x => x.Taikhoan)
                .Include(x => x.Donhangs)
                .ToListAsync();

            return View(ds);
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
            // Họ tên
            if (string.IsNullOrWhiteSpace(nv.Hoten))
            {
                ModelState.AddModelError("Hoten", "Họ tên không được để trống.");
            }

            // Email
            if (!string.IsNullOrWhiteSpace(nv.Email) &&
                await _context.Nhanviens.AnyAsync(x => x.Email == nv.Email))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
            }

            // SĐT
            if (!string.IsNullOrWhiteSpace(nv.Sdt) &&
                await _context.Nhanviens.AnyAsync(x => x.Sdt == nv.Sdt))
            {
                ModelState.AddModelError("Sdt", "Số điện thoại đã tồn tại.");
            }

            // Tài khoản
            if (nv.Matk != null &&
                await _context.Nhanviens.AnyAsync(x => x.Matk == nv.Matk))
            {
                ModelState.AddModelError("Matk", "Tài khoản này đã được gán cho nhân viên khác.");
            }
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
            // Họ tên
            if (string.IsNullOrWhiteSpace(nv.Hoten))
            {
                ModelState.AddModelError("Hoten", "Họ tên không được để trống.");
            }

            // Email
            if (!string.IsNullOrWhiteSpace(nv.Email) &&
                await _context.Nhanviens.AnyAsync(x =>
                    x.Email == nv.Email &&
                    x.Manv != nv.Manv))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
            }

            // SĐT
            if (!string.IsNullOrWhiteSpace(nv.Sdt) &&
                await _context.Nhanviens.AnyAsync(x =>
                    x.Sdt == nv.Sdt &&
                    x.Manv != nv.Manv))
            {
                ModelState.AddModelError("Sdt", "Số điện thoại đã tồn tại.");
            }

            // Tài khoản
            if (nv.Matk != null &&
                await _context.Nhanviens.AnyAsync(x =>
                    x.Matk == nv.Matk &&
                    x.Manv != nv.Manv))
            {
                ModelState.AddModelError("Matk", "Tài khoản này đã được gán cho nhân viên khác.");
            }

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
                .Include(x => x.Donhangs)
                .FirstOrDefaultAsync(x => x.Manv == id);

            if (nv == null)
                return NotFound();

            return View(nv);
        }

        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaConfirmed(int id)
        {
            var nv = await _context.Nhanviens
                .Include(x => x.Donhangs)
                .FirstOrDefaultAsync(x => x.Manv == id);

            if (nv == null)
                return NotFound();

            if (nv.Donhangs.Any())
            {
                TempData["Error"] =
                    "Nhân viên đã xử lý đơn hàng nên không thể xóa.";

                return RedirectToAction(nameof(Index));
            }

            _context.Nhanviens.Remove(nv);

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Đã xóa nhân viên.";

            return RedirectToAction(nameof(Index));
        }

        //================ CHI TIẾT =================

        public async Task<IActionResult> Chitiet(int id)
        {
            var nv = await _context.Nhanviens
                .Include(x => x.Taikhoan)
                .Include(x => x.Donhangs)
                .FirstOrDefaultAsync(x => x.Manv == id);

            if (nv == null)
                return NotFound();

            return View(nv);
        }
    }
}