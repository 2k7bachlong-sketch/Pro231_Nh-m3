
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
        public IActionResult DoiMK(string MatKhauCu, string MatKhauMoi, string NhapLai)
        {
            var matk = HttpContext.Session.GetInt32("Matk");

            if (matk == null)
            {
                return RedirectToAction("Index");
            }


            var tk = _context.Taikhoans
                .FirstOrDefault(x => x.Matk == matk);


            if (tk == null)
            {
                ViewBag.Error = "Không tìm thấy tài khoản.";
                return View("Index");
            }


            if (tk.Matkhau != MatKhauCu)
            {
                ViewBag.Error = "Mật khẩu hiện tại không đúng.";
                return View("Index");
            }


            if (MatKhauMoi != NhapLai)
            {
                ViewBag.Error = "Mật khẩu nhập lại không khớp.";
                return View("Index");
            }


            tk.Matkhau = MatKhauMoi;

            _context.SaveChanges();


            TempData["Success"] = "Đổi mật khẩu thành công.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Index(Taikhoan model)
        {
            var tk = _context.Taikhoans
                .FirstOrDefault(x =>
                    x.Tendangnhap == model.Tendangnhap &&
                    x.Matkhau == model.Matkhau);

            if (tk == null)
            {
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng.";
                return View(model);
            }

            if (tk.Trangthai != "Đã đăng kí")
            {
                ViewBag.Error = "Tài khoản đã bị khóa.";
                return View(model);
            }


            HttpContext.Session.SetString("Username", tk.Tendangnhap);
            HttpContext.Session.SetString("VaiTro", tk.Vaitro);
            HttpContext.Session.SetInt32("Matk", tk.Matk);

            var kh = _context.Khachhangs.FirstOrDefault(x => x.Matk == tk.Matk);


            if (kh != null)
            {
                HttpContext.Session.SetInt32("Makh", kh.Makh);
            }

            if (tk.Vaitro == "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            if (tk.Vaitro == "Nhân viên")
            {
                return RedirectToAction("Indexnv", "Home");
            }

            if (tk.Vaitro == "Vận chuyển")
            {
                return RedirectToAction("Index", "VanChuyen");
            }

            return RedirectToAction("Index", "Trangmua");

            


                   
        }
    }
}
