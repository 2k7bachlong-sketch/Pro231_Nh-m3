using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;

namespace duan_totnghiep.Controllers
{
    public class TrangmuaController : Controller
    {
      
        private readonly AppDbContext _context;

        public TrangmuaController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString)
        {
            ViewBag.Search = searchString;

            var sanPham = _context.Sanphams.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                sanPham = sanPham.Where(x => x.Tensp.Contains(searchString));
            }

            return View(sanPham.ToList());

        }
    }
}
