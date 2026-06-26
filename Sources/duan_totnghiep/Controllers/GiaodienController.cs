using duan_totnghiep.Models;
using Microsoft.AspNetCore.Mvc;

namespace duan_totnghiep.Controllers
{
    public class GiaodienController : Controller
    {
              public IActionResult Index()
        {
            List<Sanpham> dsSanPham = new List<Sanpham>()
            {
                new Sanpham
                {
                    Masp = 1,
                    Tensp = "Giày Grand sport",
                    Gia = 720000,
                    Hinhanh = "giay1.png"
                },

                new Sanpham
                {
                    Masp = 2,
                    Tensp = "Giày da nữ",
                    Gia = 250000,
                    Hinhanh = "giay2.png"
                },

                new Sanpham
                {
                    Masp = 3,
                    Tensp = "Giày Convert chuck taylor",
                    Gia = 320000,
                    Hinhanh = "giay3.png"
                }
            };

            return View(dsSanPham);
        }
    }
}

