using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;

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
                                    string NhapLaiMatKhau,
                                    string Vaitro)
        {
            if (MatKhau != NhapLaiMatKhau)
            {
                ViewBag.Loi = "Mật khẩu không khớp";
                return View("Index");
            }

            Taikhoan tk = new Taikhoan()
            {
                Tendangnhap = TenDangNhap,
                Matkhau = MatKhau,
                Vaitro = Vaitro
            };

            _context.Taikhoans.Add(tk);
            _context.SaveChanges();

            TempData["ThanhCong"] = "Đăng ký thành công";
            return RedirectToAction("Index", "Taikhoan");
        }
    }
}
