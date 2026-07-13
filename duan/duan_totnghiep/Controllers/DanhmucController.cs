using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace duan_totnghiep.Controllers
    {
        public class DanhmucController : Controller
        {
            private readonly AppDbContext _context;

            public DanhmucController(AppDbContext context)
            {
                _context = context;
            }
        // Trang sản phẩm khuyến mãi
        public async Task<IActionResult> SanPhamKhuyenMai()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            var ds = await _context.Sanphams
                .Include(x => x.Khuyenmai)
                .Include(x => x.Thuonghieu)
                .Include(x => x.Danhmuc)
                .Where(x => x.Khuyenmai != null
                    && x.Khuyenmai.Ngaybatdau <= today
                    && x.Khuyenmai.Ngayketthuc >= today)
                .ToListAsync();

            return View(ds);
        }
        public IActionResult List(int id, string sortOrder = "", string searchString = "", string returnController = "")
        {
            ViewBag.ReturnController = returnController;
            var sp = _context.Sanphams
                             .Where(x => x.Madm == id);

            // Tìm kiếm theo tên sản phẩm
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                sp = sp.Where(x => x.Tensp.Contains(searchString));
            }

            // Sắp xếp
            switch (sortOrder)
            {
                case "price_asc":
                    sp = sp.OrderBy(x => x.Gia);
                    break;

                case "price_desc":
                    sp = sp.OrderByDescending(x => x.Gia);
                    break;

                default:
                    sp = sp.OrderByDescending(x => x.Ngaytao);
                    break;
            }

            ViewBag.Sort = sortOrder;
            ViewBag.Search = searchString;

            var dm = _context.Danhmucs.FirstOrDefault(x => x.Madm == id);

            ViewBag.TenDanhMuc = dm?.Tendm;

            return View(sp.ToList());
        }


        // DANH SÁCH
        public async Task<IActionResult> Index(string searchString)
        {
            var query = _context.Danhmucs
                                .Include(x => x.Sanphams)
                                .AsQueryable();

            // Tìm theo tên hoặc mã
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(x =>
                    x.Tendm.Contains(searchString) ||
                    x.Madm.ToString().Contains(searchString));
            }

            return View(await query.ToListAsync());
        }

        // THÊM - GET
        public IActionResult Them()
            {
                return View();
            }

            // THÊM - POST
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Them(Danhmuc dm)
            {
                if (ModelState.IsValid)
                {
                    _context.Danhmucs.Add(dm);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(dm);
            }

            // SỬA - GET
            public async Task<IActionResult> Sua(int id)
            {
                var dm = await _context.Danhmucs.FindAsync(id);
                if (dm == null) return NotFound();

                return View(dm);
            }

            // SỬA - POST
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Sua(Danhmuc dm)
            {
                if (ModelState.IsValid)
                {
                    _context.Danhmucs.Update(dm);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(dm);
            }

            // XÓA - GET
            public async Task<IActionResult> Xoa(int id)
            {
                var dm = await _context.Danhmucs.FindAsync(id);
                if (dm == null) return NotFound();

                return View(dm);
            }

            // XÓA - POST
            [HttpPost, ActionName("Xoa")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> XacNhanXoa(int Madm)
            {
                var dm = await _context.Danhmucs.FindAsync(Madm);
                if (dm != null)
                {
                    _context.Danhmucs.Remove(dm);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }

            // CHI TIẾT
            public async Task<IActionResult> Chitiet(int id)
            {
                var dm = await _context.Danhmucs
                    .Include(x => x.Sanphams)
                    .FirstOrDefaultAsync(x => x.Madm == id);

                if (dm == null) return NotFound();

                return View(dm);
            }
        }
    }


