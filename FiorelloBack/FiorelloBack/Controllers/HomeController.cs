using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiorelloBack.DAL;
using FiorelloBack.Models;
using FiorelloBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FiorelloBack.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM
            {
              Sliders=_db.Sliders.ToList(),
              SliderContents=_db.SliderContents.FirstOrDefault(),
              Categories=_db.Categories.Where(c=>c.IsDeleted==false).ToList(),
               Surprises = _db.Surprises.ToList(),
                Experts = _db.Experts.ToList(),
                Blogs = _db.Blogs.ToList(),
            };
            return View(homeVM);
        }

        public async  Task<IActionResult> AddBasket(int id)
        {
            Product product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();
            List<BasketVM> basket;
            if (Request.Cookies["basket"] != null)
            {
                basket=JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            else
            {
                basket=new List<BasketVM>();
            }
            BasketVM isExist = basket.FirstOrDefault(p => p.Id == id);
            if (isExist == null)
            {
                basket.Add(new BasketVM
                {
                    Id = id,
                    Count = 1
                });
            }
            else
            {
                isExist.Count += 1;
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
            return RedirectToAction(nameof(Index));
            
        }



        public async Task<IActionResult> Basket()
        {
           
            List<BasketVM> dbBasket = new List<BasketVM>();
            ViewBag.Total = 0;
            if (Request.Cookies["basket"] != null)
            {

                List<BasketVM> basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);

                foreach (BasketVM pro in basket)
             {
                Product dbProduct = await _db.Products.FindAsync(pro.Id);
                pro.Title = dbProduct.Title;
                pro.Price = dbProduct.Price*pro.Count;
                pro.Image = dbProduct.Image;
                dbBasket.Add(pro);
                ViewBag.Total += pro.Price;
             }
            }



            return View(dbBasket);
        }
        public IActionResult RemoveItem(int id)
        {
            List<BasketVM> basket = new List<BasketVM>();

            basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            BasketVM remove = basket.FirstOrDefault(p => p.Id == id);
            basket.Remove(remove);
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));

            return RedirectToAction(nameof(Basket));
        }
    }
}
