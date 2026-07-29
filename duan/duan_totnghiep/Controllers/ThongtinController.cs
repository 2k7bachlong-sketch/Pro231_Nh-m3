using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace duan_totnghiep.Controllers
{
    public class ThongTinController : Controller
    {
        private readonly AppDbContext _context;

        public ThongTinController(AppDbContext context)
        {
            _context = context;
        }

        // ================= THÔNG TIN =================

        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Index", "Taikhoan");

            var tk = _context.Taikhoans
                .AsNoTracking()
                .FirstOrDefault(x => x.Tendangnhap == username);

            if (tk == null)
                return RedirectToAction("Index", "Taikhoan");

            ViewBag.Matk = tk.Matk;
            ViewBag.TenDangNhap = tk.Tendangnhap;
            ViewBag.VaiTro = tk.Vaitro;
            ViewBag.TrangThai = tk.Trangthai;

            if (tk.Vaitro == "người dùng")
            {
                var kh = _context.Khachhangs
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Matk == tk.Matk);

                if (kh != null)
                {
                    ViewBag.HoTen = kh.Hoten ?? "";
                    ViewBag.Email = kh.Email ?? "";
                    ViewBag.SDT = kh.Sdt ?? "";
                    ViewBag.DiaChi = kh.Diachi ?? "";
                }
            }
            else
            {
                var nv = _context.Nhanviens
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Matk == tk.Matk);

                if (nv != null)
                {
                    ViewBag.HoTen = nv.Hoten ?? "";
                    ViewBag.Email = nv.Email ?? "";
                    ViewBag.SDT = nv.Sdt ?? "";
                    ViewBag.DiaChi = nv.Diachi ?? "";
                    ViewBag.ChucVu = nv.Chucvu ?? "";
                }
            }

            return View();
        }

        [HttpPost]
        public IActionResult UpdateInfo(
            string HoTen,
            string Email,
            string SDT,
            string DiaChi)
        {
            try
            {
                var username = HttpContext.Session.GetString("Username");

                if (string.IsNullOrEmpty(username))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Phiên đăng nhập đã hết."
                    });
                }

                var tk = _context.Taikhoans
                    .FirstOrDefault(x => x.Tendangnhap == username);

                if (tk == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Không tìm thấy tài khoản."
                    });
                }

                if (string.IsNullOrWhiteSpace(HoTen))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Họ tên không được để trống."
                    });
                }

                if (tk.Vaitro == "người dùng")
                {
                    var kh = _context.Khachhangs
                        .FirstOrDefault(x => x.Matk == tk.Matk);

                    if (kh == null)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Không tìm thấy khách hàng."
                        });
                    }

                    kh.Hoten = HoTen.Trim();
                    kh.Email = Email?.Trim();
                    kh.Sdt = SDT?.Trim();
                    kh.Diachi = DiaChi?.Trim();
                }
                else
                {
                    var nv = _context.Nhanviens
                        .FirstOrDefault(x => x.Matk == tk.Matk);

                    if (nv == null)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Không tìm thấy nhân viên"
                        });
                    }

                    nv.Hoten = HoTen.Trim();
                    nv.Email = Email?.Trim();
                    nv.Sdt = SDT?.Trim();
                    nv.Diachi = DiaChi?.Trim();
                }

                _context.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = "Cập nhật thông tin thành công."
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // ================= ĐỔI MẬT KHẨU =================

        [HttpPost]
        public IActionResult ChangePassword(
            string MatKhauCu,
            string MatKhauMoi,
            string NhapLai)
        {
            try
            {
                var username = HttpContext.Session.GetString("Username");

                if (string.IsNullOrEmpty(username))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Phiên đăng nhập đã hết."
                    });
                }

                var tk = _context.Taikhoans
                    .FirstOrDefault(x => x.Tendangnhap == username);

                if (tk == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Không tìm thấy tài khoản."
                    });
                }

                if (string.IsNullOrWhiteSpace(MatKhauCu))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Vui lòng nhập mật khẩu cũ."
                    });
                }

                if (tk.Matkhau != MatKhauCu)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Mật khẩu cũ không đúng."
                    });
                }

                if (string.IsNullOrWhiteSpace(MatKhauMoi))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Vui lòng nhập mật khẩu mới."
                    });
                }

                if (MatKhauMoi.Length < 6)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Mật khẩu phải từ 6 ký tự trở lên."
                    });
                }

                if (MatKhauMoi != NhapLai)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Mật khẩu nhập lại không khớp."
                    });
                }

                if (MatKhauCu == MatKhauMoi)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Mật khẩu mới phải khác mật khẩu cũ."
                    });
                }

                tk.Matkhau = MatKhauMoi;

                _context.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = "Đổi mật khẩu thành công."
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}