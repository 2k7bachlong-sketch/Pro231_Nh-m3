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
                                    string Email,
                                    string MatKhau,
                                    string NhapLaiMatKhau)
        {
            // 1. Check rỗng
            // Thiếu dữ liệu
            if (string.IsNullOrWhiteSpace(TenDangNhap) ||
                string.IsNullOrWhiteSpace(SoDienThoai) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(MatKhau) ||
                string.IsNullOrWhiteSpace(NhapLaiMatKhau))
            {
                ViewBag.Loi = "Vui lòng nhập đầy đủ thông tin.";
                return View("Index");
            }

            // 2. Check mật khẩu khớp
            if (MatKhau != NhapLaiMatKhau)
            {
                ViewBag.Loi = "Mật khẩu không khớp";
                return View("Index");
            }
            // Kiểm tra Email đã tồn tại
            var checkEmail = _context.Khachhangs
                .FirstOrDefault(x => x.Email == Email);

            if (checkEmail != null)
            {
                ViewBag.Loi = "Email đã được sử dụng";
                return View("Index");
            }

            // Kiểm tra SĐT đã tồn tại
            var checkSDT = _context.Khachhangs
                .FirstOrDefault(x => x.Sdt == SoDienThoai);

            if (checkSDT != null)
            {
                ViewBag.Loi = "Số điện thoại đã được sử dụng";
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

            // Tạo hồ sơ khách hàng
           
          Khachhang kh = new Khachhang()
          {
              Hoten = TenDangNhap,
              Email = Email,
              Sdt = SoDienThoai,
              Matk = tk.Matk
          };
            _context.Khachhangs.Add(kh);
            _context.SaveChanges();

            TempData["ThanhCong"] = "Đăng ký thành công";
            return RedirectToAction("Index", "Taikhoan");
        }
    }
}