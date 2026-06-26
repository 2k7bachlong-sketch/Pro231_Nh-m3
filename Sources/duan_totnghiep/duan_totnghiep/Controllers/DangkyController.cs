using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace duan_totnghiep.Controllers
{
    public class DangkyController : Controller
    {
        private readonly AppDbContext _context;

        public DangkyController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DangKy(string TenDangNhap,
                                    string SoDienThoai,
                                    string MatKhau,
                                    string NhapLaiMatKhau)
        {
            // 1. Check rỗng
            if (string.IsNullOrEmpty(TenDangNhap) ||
                string.IsNullOrEmpty(MatKhau))
            {
                ViewBag.Loi = "Vui lòng nhập đầy đủ thông tin";
                return View("Index");
            }

            // 2. Check mật khẩu khớp
            if (MatKhau != NhapLaiMatKhau)
            {
                ViewBag.Loi = "Mật khẩu không khớp";
                return View("Index");
            }

            // 3. Check trùng tài khoản
            var check = _context.Taikhoans
                .FirstOrDefault(x => x.Tendangnhap == TenDangNhap);

            if (check != null)
            {
                ViewBag.Loi = "Tên đăng nhập đã tồn tại";
                return View("Index");
            }

            // 4. Tạo tài khoản mới
            Taikhoan tk = new Taikhoan()
            {
                Tendangnhap = TenDangNhap,
                Matkhau = MatKhau, // nên hash nếu làm thật
                Trangthai = "Đã đăng kí",
                Vaitro = "người dùng"
            };

            _context.Taikhoans.Add(tk);
            _context.SaveChanges();

            TempData["ThanhCong"] = "Đăng ký thành công";
            return RedirectToAction("Index", "Taikhoan");
        }
    }
}