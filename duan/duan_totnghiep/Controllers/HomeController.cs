using System.Diagnostics;
using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;

namespace duan_totnghiep.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(AppDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.TongSanPham = _context.Sanphams.Count();

            ViewBag.TongKhachHang = _context.Khachhangs.Count();

            ViewBag.TongDonHang = _context.Donhangs.Count();

            ViewBag.TongKhuyenMai = _context.Khuyenmais.Count();

            ViewBag.TongDanhMuc = _context.Danhmucs.Count();

            ViewBag.TongThuongHieu = _context.Thuonghieus.Count();

            ViewBag.TonKho = _context.Sanphams.Sum(x => x.Soluongton ?? 0);

            ViewBag.DoanhThu = _context.Donhangs.Any()
                ? _context.Donhangs.Sum(x => x.Tongtien)
                : 0;

            return View();
        }

        public IActionResult Indexnv()
        {
            ViewBag.TongSanPham = _context.Sanphams.Count();

            ViewBag.TongKhachHang = _context.Khachhangs.Count();

            ViewBag.TongDonHang = _context.Donhangs.Count();

            ViewBag.TongKhuyenMai = _context.Khuyenmais.Count();

            ViewBag.TongDanhMuc = _context.Danhmucs.Count();

            ViewBag.TongThuongHieu = _context.Thuonghieus.Count();

            ViewBag.TonKho = _context.Sanphams.Sum(x => x.Soluongton ?? 0);

            ViewBag.DoanhThu = _context.Donhangs.Any()
                ? _context.Donhangs.Sum(x => x.Tongtien)
                : 0;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}