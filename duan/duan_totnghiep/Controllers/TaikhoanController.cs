using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;

namespace duan_totnghiep.Controllers
{
    public class TaikhoanController : Controller
    {
        private readonly AppDbContext _context;

        public TaikhoanController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Taikhoan model)
        {
            // Tài khoản Admin cố định
            if (model.Tendangnhap == "Admin" &&
                model.Matkhau == "123456")
            {
                HttpContext.Session.SetString("Username", "Admin");
                HttpContext.Session.SetString("Role", "Admin");

                return RedirectToAction("Index", "Home");
            }
            // Nhân viên
            if (model.Tendangnhap == "Nhanvien" &&
                model.Matkhau == "12345")
            {
                HttpContext.Session.SetString("Username", "Nhanvien");
                HttpContext.Session.SetString("Role", "Nhanvien");

                return RedirectToAction("Indexnv", "Home");
            }

            // Mọi tài khoản khác đều vào giao diện người dùng
            // Tìm tài khoản trong database
            var tk = _context.Taikhoans
                             .FirstOrDefault(x =>
                                 x.Tendangnhap == model.Tendangnhap &&
                                 x.Matkhau == model.Matkhau);

            if (tk != null)
            {
                HttpContext.Session.SetString("Username", tk.Tendangnhap);
                HttpContext.Session.SetString("Role", "User");

                // Lưu mã khách hàng vào Session
                HttpContext.Session.SetInt32("Makh", tk.Matk);

                return RedirectToAction("Index", "Trangmua");
            }

            ViewBag.Error = "Vui lòng nhập đầy đủ thông tin";
            return View("Index");
        }
    }
}
