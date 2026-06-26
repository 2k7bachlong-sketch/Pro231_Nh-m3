using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace duan_totnghiep.Controllers
{
    public class ThuonghieuController : Controller
    {
        private readonly AppDbContext _context;

        public ThuonghieuController(AppDbContext context)
        {
            _context = context;
        }

        //Danh sách
        public IActionResult Index()
        {
            return View(_context.Thuonghieus.ToList());
        }

        //Chi tiết
        public IActionResult Chitiet(int id)
        {
            var th = _context.Thuonghieus.Find(id);

            if (th == null)
                return NotFound();

            return View(th);
        }

        //GET
        public IActionResult Them()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Them(Thuonghieu th)
        {
            if (ModelState.IsValid)
            {
                _context.Thuonghieus.Add(th);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(th);
        }

        //GET
        public IActionResult Sua(int id)
        {
            var th = _context.Thuonghieus.Find(id);

            if (th == null)
                return NotFound();

            return View(th);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Sua(int id, Thuonghieu th)
        {
            if (id != th.Math)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(th);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(th);
        }

        //GET
        public IActionResult Xoa(int id)
        {
            var th = _context.Thuonghieus.Find(id);

            if (th == null)
                return NotFound();

            return View(th);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Xacnhanxoa(int id)
        {
            var th = _context.Thuonghieus.Find(id);

            if (th != null)
            {
                _context.Thuonghieus.Remove(th);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}