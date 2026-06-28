using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace duan_totnghiep.Controllers
{
    public class GiohangController : Controller
    {
        private readonly AppDbContext _context;

        public GiohangController(AppDbContext context)
        {
            _context = context;
        }

        // Hiển thị giỏ hàng
        public IActionResult Index()
        {
            int? maKhachHang = HttpContext.Session.GetInt32("Makh");

            // Nếu chưa đăng nhập thì hiển thị giỏ trống
            if (maKhachHang == null)
            {
                return View(new List<Giohang>());
            }

            var gioHang = _context.Giohangs
                                  .Include(x => x.Sanpham)
                                  .Where(x => x.Makh == maKhachHang.Value)
                                  .ToList();

            return View(gioHang);
        }

        // Thêm vào giỏ hàng
        public IActionResult ThemVaoGio(int id)
        {
            // Lấy mã khách hàng từ Session
            int? maKhachHang = HttpContext.Session.GetInt32("Makh");     


            // Chưa đăng nhập
            if (maKhachHang == null)
            {
                return RedirectToAction("Index", "Giohang");
                // Hoặc:
                // return Content("Bạn cần đăng nhập trước");
            }

            // Kiểm tra sản phẩm
            var sp = _context.Sanphams
                             .FirstOrDefault(x => x.Masp == id);

            if (sp == null)
            {
                return Content("Không tìm thấy sản phẩm");
            }

            // Kiểm tra sản phẩm đã có trong giỏ chưa
            var item = _context.Giohangs
                .FirstOrDefault(x => x.Masp == id
                                  && x.Makh == maKhachHang.Value);

            // Nếu chưa có thì thêm mới
            if (item == null)
            {
                Giohang gh = new Giohang()
                {
                    Makh = maKhachHang.Value,
                    Masp = id,
                    Soluong = 1
                };

                _context.Giohangs.Add(gh);
            }
            // Nếu đã có thì tăng số lượng
            else
            {
                item.Soluong += 1;
            }

            _context.SaveChanges();

            TempData["Success"] = "Đã thêm sản phẩm vào giỏ hàng!";
            return RedirectToAction("Index", "Trangmua");
        }

        // Xóa sản phẩm khỏi giỏ
        public IActionResult Xoa(int id)
        {
            var item = _context.Giohangs
                .FirstOrDefault(x => x.Magh == id);

            if (item != null)
            {
                _context.Giohangs.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // Tăng số lượng
        public IActionResult Tang(int id)
        {
            var item = _context.Giohangs
                .FirstOrDefault(x => x.Magh == id);

            if (item != null)
            {
                item.Soluong++;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // Giảm số lượng
        public IActionResult Giam(int id)
        {
            var item = _context.Giohangs
                .FirstOrDefault(x => x.Magh == id);

            if (item != null)
            {
                item.Soluong--;

                if (item.Soluong <= 0)
                {
                    _context.Giohangs.Remove(item);
                }

                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}

