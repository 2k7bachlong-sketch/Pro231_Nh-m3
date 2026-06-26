    using duan_totnghiep.Models;
    using Microsoft.AspNetCore.Mvc;

namespace duan_totnghiep.Controllers
{
    public class GiaodienController : Controller
    {

        private readonly AppDbContext _context;

        public GiaodienController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var dsSanPham = _context.Sanphams.ToList();

            return View(dsSanPham);
        }
    }
}

