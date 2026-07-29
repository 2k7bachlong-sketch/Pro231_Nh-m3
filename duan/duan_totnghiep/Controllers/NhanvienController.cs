using duan_totnghiep.Filters;
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

        public async Task<IActionResult> Index(string search)
        {
            var ds = _context.Nhanviens
                .Include(x => x.Taikhoan)
                .Include(x => x.Donhangs)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                ds = ds.Where(x =>
                    x.Hoten.Contains(search) ||
                    x.Sdt.Contains(search));
            }

            return View(await ds.ToListAsync());
        }

        //================ THÊM =================

        public IActionResult Them()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(
     Nhanvien nv,
     string TenDangNhap,
     string MatKhau)
        {
            if (string.IsNullOrWhiteSpace(nv.Hoten))
                ModelState.AddModelError("Hoten", "Họ tên không được để trống.");

            if (string.IsNullOrWhiteSpace(TenDangNhap))
                ModelState.AddModelError("", "Tên đăng nhập không được để trống.");

            if (string.IsNullOrWhiteSpace(MatKhau))
                ModelState.AddModelError("", "Mật khẩu không được để trống.");

            if (!string.IsNullOrWhiteSpace(nv.Email) &&
                await _context.Nhanviens.AnyAsync(x => x.Email == nv.Email))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
            }

            if (!string.IsNullOrWhiteSpace(nv.Sdt) &&
                await _context.Nhanviens.AnyAsync(x => x.Sdt == nv.Sdt))
            {
                ModelState.AddModelError("Sdt", "Số điện thoại đã tồn tại.");
            }

            if (await _context.Taikhoans.AnyAsync(x => x.Tendangnhap == TenDangNhap))
            {
                ModelState.AddModelError("", "Tên đăng nhập đã tồn tại.");
            }

            if (!ModelState.IsValid)
            {
                return View(nv);
            }

            var tk = new Taikhoan
            {
                Tendangnhap = TenDangNhap,
                Matkhau = MatKhau,
                Vaitro = nv.Chucvu,      // Nhân viên hoặc Quản lý
                Trangthai = "Đã đăng kí"
            };

            _context.Taikhoans.Add(tk);
            await _context.SaveChangesAsync();

            nv.Matk = tk.Matk;

            _context.Nhanviens.Add(nv);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã thêm nhân viên thành công.";

            return RedirectToAction(nameof(Index));
        }

        //================ SỬA =================

        public async Task<IActionResult> Sua(int id)
        {
            var nv = await _context.Nhanviens
    .Include(x => x.Taikhoan)
    .FirstOrDefaultAsync(x => x.Manv == id);

            if (nv == null)
                return NotFound();

            return View(nv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(
    int id,
    Nhanvien nv,
    string Tendangnhap,
    string Matkhau)
        {
            if (string.IsNullOrWhiteSpace(nv.Hoten))
                ModelState.AddModelError("Hoten", "Họ tên không được để trống.");

            if (!string.IsNullOrWhiteSpace(nv.Email) &&
                await _context.Nhanviens.AnyAsync(x =>
                    x.Email == nv.Email &&
                    x.Manv != nv.Manv))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
            }

            if (!string.IsNullOrWhiteSpace(nv.Sdt) &&
                await _context.Nhanviens.AnyAsync(x =>
                    x.Sdt == nv.Sdt &&
                    x.Manv != nv.Manv))
            {
                ModelState.AddModelError("Sdt", "Số điện thoại đã tồn tại.");
            }

            var nvDb = await _context.Nhanviens
                .Include(x => x.Taikhoan)
                .FirstOrDefaultAsync(x => x.Manv == id);

            if (nvDb == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(nv);

            nvDb.Hoten = nv.Hoten;
            nvDb.Sdt = nv.Sdt;
            nvDb.Email = nv.Email;
            nvDb.Diachi = nv.Diachi;
            nvDb.Chucvu = nv.Chucvu;

            if (nvDb.Taikhoan != null)
            {
                bool trung = await _context.Taikhoans.AnyAsync(x =>
                    x.Tendangnhap == Tendangnhap &&
                    x.Matk != nvDb.Taikhoan.Matk);

                if (trung)
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại.");
                    return View(nvDb);
                }

                nvDb.Taikhoan.Tendangnhap = Tendangnhap;

                if (!string.IsNullOrWhiteSpace(Matkhau))
                {
                    nvDb.Taikhoan.Matkhau = Matkhau;
                }

                nvDb.Taikhoan.Vaitro = nv.Chucvu;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật nhân viên thành công.";

            return RedirectToAction(nameof(Index));
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
                .Include(x => x.Taikhoan)
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

            // Xóa tài khoản nếu có
            if (nv.Taikhoan != null)
            {
                _context.Taikhoans.Remove(nv.Taikhoan);
            }

            // Xóa nhân viên
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

        public async Task<IActionResult> Khoa(int id)
        {
            var nv = await _context.Nhanviens
                .Include(x => x.Taikhoan)
                .FirstOrDefaultAsync(x => x.Manv == id);

            if (nv == null || nv.Taikhoan == null)
                return NotFound();

            nv.Taikhoan.Trangthai = "Đã khóa";

            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã khóa tài khoản nhân viên.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MoKhoa(int id)
        {
            var nv = await _context.Nhanviens
                .Include(x => x.Taikhoan)
                .FirstOrDefaultAsync(x => x.Manv == id);

            if (nv == null || nv.Taikhoan == null)
                return NotFound();

            nv.Taikhoan.Trangthai = "Đã đăng kí";

            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã mở khóa tài khoản.";

            return RedirectToAction(nameof(Index));
        }
    }
}