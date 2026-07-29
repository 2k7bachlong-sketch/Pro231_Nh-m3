using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace duan_totnghiep.Controllers
{
    public class TrangmuaController : Controller
    {

        private readonly AppDbContext _context;

        public TrangmuaController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> ChiTiet(int id)
        {
            var sp = await _context.Sanphams
                .Include(x => x.Danhmuc)
                .Include(x => x.Thuonghieu)
                .Include(x => x.Khuyenmai)
                .FirstOrDefaultAsync(x => x.Masp == id);

            if (sp == null)
            {
                return NotFound();
            }

            return View(sp);
        }


        public IActionResult Index(string searchString)
        {
            ViewBag.Search = searchString;

            var sanPham = _context.Sanphams
                .Include(x => x.Khuyenmai)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                sanPham = sanPham.Where(x => x.Tensp.Contains(searchString));
            }

            int? maKh = HttpContext.Session.GetInt32("Makh");

            if (maKh != null)
            {
                var donHangs = _context.Donhangs
                    .Where(x => x.Makh == maKh)
                    .OrderByDescending(x => x.Ngaydat)
                    .ToList();

                ViewBag.DonHangs = donHangs;
            }

            return View(sanPham.ToList());
        }

        public IActionResult KhuyenMai()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            var ds = _context.Khuyenmais
                .Where(x => x.Ngayketthuc >= today)
                .OrderByDescending(x => x.Ngaybatdau)
                .ToList();

            return View(ds);
        }
        public IActionResult VongQuay()
        {


            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            var khuyenMai = _context.Khuyenmais
                .Where(x => x.Ngaybatdau <= today &&
                            x.Ngayketthuc >= today)
                .OrderBy(x => x.Makm)
                .ToList();

            return View(khuyenMai);
        }
            [HttpPost]
        [HttpPost]
        public IActionResult Quay()
        {
            try
            {
                int? maKh = HttpContext.Session.GetInt32("Makh");

                if (maKh == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Vui lòng đăng nhập."
                    });
                }

                Random rd = new Random();

                DateOnly today = DateOnly.FromDateTime(DateTime.Today);

                var dsKhuyenMai = _context.Khuyenmais
                    .Where(x => x.Ngaybatdau <= today &&
                                x.Ngayketthuc >= today)
                    .ToList();

                if (!dsKhuyenMai.Any())
                {
                    return Json(new
                    {
                        success = false,
                        message = "Chưa có khuyến mãi."
                    });
                }

                /*var km = dsKhuyenMai[rd.Next(dsKhuyenMai.Count)];

                _context.KhachhangKhuyenmais.Add(new KhachhangKhuyenmai
                {
                    Makh = maKh.Value,
                    Makm = km.Makm,
                    Dasudung = false,
                    Ngaynhan = DateTime.Now
                });

                _context.SaveChanges();*/
                var km = dsKhuyenMai[rd.Next(dsKhuyenMai.Count)];

                bool daCo = _context.KhachhangKhuyenmais.Any(x =>
                    x.Makh == maKh.Value &&
                    x.Makm == km.Makm);

                if (!daCo)
                {
                    _context.KhachhangKhuyenmais.Add(new KhachhangKhuyenmai
                    {
                        Makh = maKh.Value,
                        Makm = km.Makm,
                        Dasudung = false,
                        Ngaynhan = DateTime.Now
                    });

                    _context.SaveChanges();
                }

                return Json(new
                {
                    success = true,
                    makm = km.Makm,
                    ten = km.Tenkm,
                    giam = km.Phantramgiam
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.ToString()
                });
            }
        }
    }
}
