using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace duan_totnghiep.Controllers
{
    public class VoucherController : Controller
    {
        private readonly AppDbContext _context;

        public VoucherController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int? maKh = HttpContext.Session.GetInt32("Makh");

            if (maKh == null)
                return RedirectToAction("Index", "Taikhoan");

            var voucher = _context.KhachhangKhuyenmais
                .Include(x => x.Khuyenmai)
                .Where(x => x.Makh == maKh)
                .OrderByDescending(x => x.Ngaynhan)
                .ToList();

            return View(voucher);
        }
    }
}