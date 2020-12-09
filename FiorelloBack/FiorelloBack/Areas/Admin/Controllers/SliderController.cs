using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FiorelloBack.DAL;
using FiorelloBack.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FiorelloBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.Sliders.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (slider.Photo == null)
            {
                return View();
            }
            if (!slider.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Please select image type");
                return View();
            }
            if (slider.Photo.Length / 1024 > 200)
            {
                ModelState.AddModelError("Photo", "Max 200kb");
                return View();
            }

            
            string fileName = Guid.NewGuid().ToString() + slider.Photo.FileName;
            string path = Path.Combine(_env.WebRootPath, "img", fileName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await slider.Photo.CopyToAsync(fileStream);
               
            }
            slider.Image = fileName;
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
       
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Slider slider =await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            return View(slider);
          
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) return NotFound();
            Slider slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            string path = Path.Combine(_env.WebRootPath, "img", slider.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            Slider slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Update")]
        public async Task<IActionResult> Update(int? id, Slider slider)
        {

            if (id == null) return NotFound();
            if (slider == null) return NotFound();
            Slider slide = await _context.Sliders.FindAsync(id);
            Slider isExist = _context.Sliders.FirstOrDefault(s => s.Image.ToLower() == slide.Image.ToLower());
            if (isExist != null)
            {
                if (isExist != slide)
                {
                    ModelState.AddModelError("Name", "Artiq bu adda  movcuddur");
                    return View();
                }
            }
            slide.Image = slide.Image;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
