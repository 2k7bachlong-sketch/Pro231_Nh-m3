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
            public async Task<IActionResult> Index(string search)
            {
            var ds = _context.Khachhangs
                  .Include(x => x.Donhangs)
                  .Include(x => x.Taikhoan)
                  .AsQueryable();

            if (!string.IsNullOrEmpty(search))
                {
                    ds = ds.Where(x =>
                        x.Hoten.Contains(search) ||
                        x.Sdt.Contains(search));
                }

                return View(await ds.ToListAsync());
            }

            // Chi tiết
            public async Task<IActionResult> ChiTiet(int id)
            {
                var kh = await _context.Khachhangs
                    .Include(x => x.Donhangs)
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
            var kh = await _context.Khachhangs
                .Include(x => x.Taikhoan)
                .FirstOrDefaultAsync(x => x.Makh == id);

            if (kh == null)
                return NotFound();

            if (kh.Hoten == "Khách vãng lai")
            {
                TempData["Error"] = "Không thể chỉnh sửa khách vãng lai.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TenDangNhap = kh.Taikhoan?.Tendangnhap;
            ViewBag.MatKhau = kh.Taikhoan?.Matkhau;

            return View(kh);
        }

        // POST Sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(int id, Khachhang model, string TenDangNhap, string MatKhau)
        {
            if (id != model.Makh)
                return NotFound();

            var khDb = await _context.Khachhangs
     .Include(x => x.Taikhoan)
     .FirstOrDefaultAsync(x => x.Makh == id);

            if (khDb == null)
                return NotFound();

            khDb.Hoten = model.Hoten;
            khDb.Email = model.Email;
            khDb.Sdt = model.Sdt;
            khDb.Diachi = model.Diachi;

            if (khDb.Taikhoan != null)
            {
                khDb.Taikhoan.Tendangnhap = TenDangNhap;
                khDb.Taikhoan.Matkhau = MatKhau;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET Xóa
        public async Task<IActionResult> Xoa(int Makh)
            {
                var kh = await _context.Khachhangs
                    .FirstOrDefaultAsync(x => x.Makh == Makh);

                if (kh == null)
                    return NotFound();
                if (kh.Hoten == "Khách vãng lai")
                {
                    TempData["Error"] = "Không thể xóa khách vãng lai.";
                    return RedirectToAction(nameof(Index));
                }

                return View(kh);
            }

        // POST Xóa
        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaXacNhan(int Makh)
        {
            var kh = await _context.Khachhangs
                .Include(x => x.Donhangs)
                .Include(x => x.Taikhoan)
                .FirstOrDefaultAsync(x => x.Makh == Makh);

            if (kh == null)
                return NotFound();

            if (kh.Donhangs.Any())
            {
                TempData["Error"] =
                    "Khách hàng đã có đơn hàng nên không thể xóa.";

                return RedirectToAction(nameof(Index));
            }

            // Nếu khách có tài khoản thì xóa luôn tài khoản
            if (kh.Taikhoan != null)
            {
                _context.Taikhoans.Remove(kh.Taikhoan);
            }

            _context.Khachhangs.Remove(kh);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã xóa khách hàng.";

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Khoa(int id)
        {
            var kh = await _context.Khachhangs
                .Include(x => x.Taikhoan)
                .FirstOrDefaultAsync(x => x.Makh == id);

            if (kh == null)
                return NotFound();

            if (kh.Taikhoan == null)
            {
                TempData["Error"] = "Khách hàng này không có tài khoản.";
                return RedirectToAction(nameof(Index));
            }

            kh.Taikhoan.Trangthai = "Đã khóa";

            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã khóa tài khoản.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MoKhoa(int id)
        {
            var kh = await _context.Khachhangs
                .Include(x => x.Taikhoan)
                .FirstOrDefaultAsync(x => x.Makh == id);

            if (kh == null)
                return NotFound();

            if (kh.Taikhoan == null)
            {
                TempData["Error"] = "Khách hàng này không có tài khoản.";
                return RedirectToAction(nameof(Index));
            }

            kh.Taikhoan.Trangthai = "Đã đăng kí";

            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã mở khóa tài khoản.";

            return RedirectToAction(nameof(Index));
        }
    }
}