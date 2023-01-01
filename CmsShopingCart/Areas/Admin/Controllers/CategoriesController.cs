using CmsShopingCart.Infrastructure;
using CmsShopingCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShopingCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly CmsShopingCartContext context;
        public CategoriesController(CmsShopingCartContext context)
        {
            this.context = context;
        }
        //GET /admin/categories
        public async Task<IActionResult> Index()
        {
            return View(await context.Categories.OrderBy(x => x.Sorting).ToListAsync());
        }
        //GET /admin/categories/create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        //POST /admin/categories/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");
                category.Sorting = 100;

                var slug = await context.Categories.FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The category already exist");
                    return View(category);
                }
                context.Add(category);
                await context.SaveChangesAsync();
                TempData["Success"] = "The category has been added!";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        //GET /admin/categories/edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Category category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        //POST /admin/categories/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");
                var slug = await context.Categories.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The category already exist");
                    return View(category);
                }
                context.Update(category);
                await context.SaveChangesAsync();
                TempData["Success"] = "The category has been edited!";
                return RedirectToAction("Edit", new { id});
            }
            return View(category);
        }
        //GET /admin/categories/delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                TempData["Error"] = "The category does not exist";
                // return NotFound();
            }
            else
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                TempData["Success"] = "The category has been deleted!";

            }
            return RedirectToAction("Index");
        }
        //POST /admin/categories/reorder
        [HttpPost]
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;

            foreach (var categoryId in id)
            {
                Category category = await context.Categories.FindAsync(categoryId);
                category.Sorting = count;
                context.Update(category);
                await context.SaveChangesAsync();
                count++;
            }
            return Ok();
        }
    }
}
