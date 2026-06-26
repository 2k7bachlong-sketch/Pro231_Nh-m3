using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;

namespace duan_totnghiep.Controllers
{
    public class DangnhapController : Controller
    {
        private readonly AppDbContext _context;

        public DangnhapController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Taikhoan model)
        {
            var tk = _context.Taikhoans.FirstOrDefault(x =>
                x.Tendangnhap == model.Tendangnhap &&
                x.Matkhau == model.Matkhau);

            if (tk != null)
            {
                HttpContext.Session.SetString("Username", tk.Tendangnhap);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
