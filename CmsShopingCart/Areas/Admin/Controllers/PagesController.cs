using CmsShopingCart.Infrastructure;
using CmsShopingCart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShopingCart.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin, editor")]
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly CmsShopingCartContext context;
        public PagesController(CmsShopingCartContext context)
        {
            this.context = context;
        }
        //GET /admin/pages
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IQueryable<Page> pages = from p in context.Pages orderby p.Sorting select p;
            List<Page> pagesList = await pages.ToListAsync();
            return View(pagesList);
        }
        //GET /admin/pages/details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Page page = await context.Pages.FirstOrDefaultAsync(x => x.Id == id);
            if (page == null) {
                return NotFound();
            }
            return View(page);
        }
        //GET /admin/pages/create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        //POST /admin/pages/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Page page)
        {
            if (ModelState.IsValid) {
                page.Slug = page.Title.ToLower().Replace(" ","-");
                page.Sorting = 100;

                var slug = await context.Pages.FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null) {
                    ModelState.AddModelError("", "The page already exist");
                    return View(page);
                }
                context.Add(page);
                await context.SaveChangesAsync();
                TempData["Success"] = "The Page has been added!";
                return RedirectToAction("Index");
            }
            return View(page);
        }
        //GET /admin/pages/edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Page page = await context.Pages.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }
        //POST /admin/pages/edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Id ==1 ? "home": page.Title.ToLower().Replace(" ", "-");
                var slug = await context.Pages.Where(x=>x.Id != page.Id).FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The page already exist");
                    return View(page);
                }
                context.Update(page);
                await context.SaveChangesAsync();
                TempData["Success"] = "The Page has been edited!";
                return RedirectToAction("Edit", new { id = page.Id});
            }
            return View(page);
        }
        //GET /admin/pages/delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Page page = await context.Pages.FindAsync(id);
            if (page == null)
            {
                TempData["Error"] = "The page does not exist";
                // return NotFound();
            }
            else {
                context.Pages.Remove(page);
                await context.SaveChangesAsync();
                TempData["Success"] = "The Page has been deleted!";

            }
            return RedirectToAction("Index");
        }
        //POST /admin/pages/reorder
        [HttpPost]
        public async Task<IActionResult> Reorder(int[]  id)
        {
            int count = 1;

            foreach (var pageId in id) {
                Page page = await context.Pages.FindAsync(pageId);
                page.Sorting = count;
                context.Update(page);
                await context.SaveChangesAsync();
                count++;
            }
            return Ok();
        }
    }
}
