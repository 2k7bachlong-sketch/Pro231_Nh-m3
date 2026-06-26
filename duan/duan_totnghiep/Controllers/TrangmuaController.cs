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
            var sanphams = _context.Sanphams.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                sanphams = sanphams.Where(x =>
                    x.Tensp.Contains(searchString));
            }

            return View(sanphams.ToList());
        }
    }
}
