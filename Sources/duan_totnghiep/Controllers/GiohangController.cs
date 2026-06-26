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
      
            var gioHang = _context.Giohangs.ToList();
            return View(gioHang);
        
        }

        // Thêm vào giỏ hàng
        public IActionResult ThemVaoGio(int id)
        {
            // id là mã sản phẩm

            int maKhachHang = 1; // Tạm thời, sau này lấy từ Session

            var item = _context.Giohangs
                .FirstOrDefault(x => x.Masp == id
                                  && x.Makh == maKhachHang);

            if (item == null)
            {
                Giohang gh = new Giohang()
                {
                    Makh = maKhachHang,
                    Masp = id,
                    Soluong = 1
                };

                _context.Giohangs.Add(gh);
            }
            else
            {
                item.Soluong += 1;
                _context.Giohangs.Update(item);
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
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
        public IActionResult TangSL(int id)
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
        public IActionResult GiamSL(int id)
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

