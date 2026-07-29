using System.Diagnostics;
using duan_totnghiep.Filters;
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


        [QuanLyOnly]
        public IActionResult Index()
        {
            ViewBag.TongSanPham = _context.Sanphams.Count();

            ViewBag.TongKhachHang = _context.Khachhangs.Count();

            ViewBag.TongDonHang = _context.Donhangs.Count();

            ViewBag.TongKhuyenMai = _context.Khuyenmais.Count();

            ViewBag.TongDanhMuc = _context.Danhmucs.Count();

            ViewBag.TongThuongHieu = _context.Thuonghieus.Count();

            ViewBag.TonKho = _context.Sanphams.Sum(x => x.Soluongton ?? 0);

            ViewBag.DoanhThu = _context.Donhangs
                .Where(x => x.Trangthai == "Đã hoàn thành")
                .Sum(x => x.Tongtien);
            ViewBag.DonHangMoi = _context.Donhangs
                .OrderByDescending(x => x.Ngaydat)
                .Take(5)
                .ToList();

            ViewBag.SanPhamMoi = _context.Sanphams
                .OrderByDescending(x => x.Ngaytao)
                .Take(5)
                .ToList();

            ViewBag.SoDonMoi = _context.Donhangs.Count(x => x.Trangthai == "Chờ xác nhận");
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

            ViewBag.DoanhThu = _context.Donhangs
                .Where(x => x.Trangthai == "Đã hoàn thành")
                .Sum(x => x.Tongtien);

            return View();
        }

        //403 
        public IActionResult AccessDenied()
        {
            return View();
        }

        //Joke
        [QuanLyOnly]
        public IActionResult Joke()
        {
            return View();
        }
        //Kiếm tra Role trả về giao diện
        public IActionResult TrangChu()
        {
            var role = HttpContext.Session.GetString("VaiTro");

            if (role == "Admin")
            {
                return RedirectToAction("Index");
            }


            if (role == "Nhân viên")
            {
                return RedirectToAction("Indexnv");
            }

            return RedirectToAction("Index", "Trangmua");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
        
    }
}
