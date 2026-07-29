using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace duan_totnghiep.Controllers
{
    public class VanchuyenController : Controller
    {
        private readonly AppDbContext _context;

        public VanchuyenController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var donDangXuLy = await _context.Donhangs
                .Include(x => x.Khachhang)
                    .Include(x => x.Chitietdonhangs)
                    .ThenInclude(x => x.Sanpham)
                    .ThenInclude(x => x.Thuonghieu)
                .Where(x =>
                    x.Trangthai == "Đã xác nhận" ||
                    x.Trangthai == "Đang giao")
                .OrderByDescending(x => x.Ngaydat)
                .ToListAsync();

            var lichSu = await _context.Donhangs
                .Include(x => x.Khachhang)
                    .Include(x => x.Chitietdonhangs)
                    .ThenInclude(x => x.Sanpham)
                    .ThenInclude(x => x.Thuonghieu)
                .Where(x =>
                    x.Trangthai == "Đã hoàn thành" ||
                    x.Trangthai == "Đã Huỷ" ||
                    x.Trangthai == "Giao thất bại")
                .OrderByDescending(x => x.Ngaydat)
                .ToListAsync();

            ViewBag.LichSu = lichSu;

            return View(donDangXuLy);
        }

        [HttpPost]
        public async Task<IActionResult> DaGiao(int id)
        {
            var dh = await _context.Donhangs.FindAsync(id);

            if (dh == null)
                return NotFound();

            dh.Trangthai = "Đã giao";

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đơn #{dh.Madh} đã giao thành công.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> GiaoThatBai(int id)
        {
            var dh = await _context.Donhangs.FindAsync(id);

            if (dh == null)
                return NotFound();

            dh.Trangthai = "Giao thất bại";

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đơn #{dh.Madh} giao thất bại.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Xoa(int id)
        {
            var dh = await _context.Donhangs.FindAsync(id);

            if (dh == null)
                return NotFound();

            _context.Donhangs.Remove(dh);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã xóa đơn hàng.";

            return RedirectToAction(nameof(Index));
        }
    }
}