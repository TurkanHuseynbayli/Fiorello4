using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiorelloBack.DAL;
using FiorelloBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Categories.Where(c=>c.IsDeleted==false).ToList());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            
            
            if (!ModelState.IsValid) return NotFound();
            bool isExist = _context.Categories.Where(c=>c.IsDeleted==false).Any(c => c.Name.ToLower() == category.Name.ToLower());
            if (isExist)
            {
                ModelState.AddModelError("Name", "bu addan var");
                return View();
            }
            category.IsDeleted = false;
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int? id)
        {
           
            if (id == null) return NotFound();
            Category category = _context.Categories.Where(c => c.IsDeleted == false).FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Category category = _context.Categories.Where(c => c.IsDeleted == false).FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) return NotFound();
            Category category = _context.Categories.Where(c=>c.IsDeleted==false).Include(c=>c.Products).FirstOrDefault(c=>c.Id==id);
            if (category == null) return NotFound();

            //_context.Categories.Remove(category);
            //await _context.SaveChangesAsync();

            category.IsDeleted = true;
            category.DeletedTime = DateTime.Now;
            foreach (Product pro in category.Products)
            {
                //pro.DeletedTime = DateTime.Now;
                pro.IsDeleted = true;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            Category category = _context.Categories.Where(c => c.IsDeleted == false).FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Update")]
        public async Task<IActionResult> Update(int? id, Category category)
        {
           
            if (id == null) return NotFound();
            if (category == null) return NotFound();
            Category categ = await _context.Categories.FindAsync(id);
            Category isExist = _context.Categories.Where(c => c.IsDeleted == false).FirstOrDefault(c => c.Name.ToLower() == category.Name.ToLower());
            if (isExist != null)
            {
                if (isExist != categ)
                {
                    ModelState.AddModelError("Name", "Artiq bu adda category movcuddur");
                    return View();
                }
            }
            categ.Name = category.Name;
            categ.Description = category.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

    }
}
