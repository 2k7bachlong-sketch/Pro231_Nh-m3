using Microsoft.AspNetCore.Mvc;

namespace duan_totnghiep.Controllers
{
    public class DangkyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // POST: Xử lý đăng ký
        [HttpPost]
        public IActionResult DangKy(string TenDangNhap,
                                    string SoDienThoai,
                                    string MatKhau,
                                    string NhapLaiMatKhau)
        {
            if (MatKhau != NhapLaiMatKhau)
            {
                ViewBag.Loi = "Mật khẩu không khớp";
                return View();
            }

            // Thêm code lưu database ở đây

            TempData["ThanhCong"] = "Đăng ký thành công";
            return RedirectToAction("DangNhap");
        }
    }
}
