using CmsShopingCart.Infrastructure;
using CmsShopingCart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShopingCart.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly CmsShopingCartContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(CmsShopingCartContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }
        //GET /admin/product
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6;
            var products = context.Products.OrderByDescending(x => x.Id)
                                            .Include(x => x.Category)
                                            .Skip((p-1)* pageSize)
                                            .Take(pageSize);
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Products.Count() / pageSize);
            return View(await products.ToListAsync());
        }
        //GET /admin/product/details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Product product = await context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        //GET /admin/product/create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x=>x.Sorting), "Id", "Name");
            return View();
        }

        //POST /admin/product/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name");
            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");
                

                var slug = await context.Products.FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The product already exist");
                    return View(product);
                }
                //upload image
                string imageName = "noimage.png";
                if (product.ImageUpload != null) {
                    string uploaDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
                    imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploaDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    
                }
                product.Image = imageName;
                context.Add(product);
                await context.SaveChangesAsync();
                TempData["Success"] = "The product has been added!";
                return RedirectToAction("Index");
            }
            return View(product);
        }

        //GET /admin/product/edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        { 
            Product product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name",product.CategoryId);
            return View(product);
        }
        //POST /admin/product/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name", product.CategoryId);
            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");
                var slug = await context.Products.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The product already exist");
                    return View(product);
                }
                //upload image
       
                if (product.ImageUpload != null)
                {
                    string uploaDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
                    if (!string.Equals(product.Image, "noimage.png")) {
                        string oldImagePath = Path.Combine(uploaDir, product.Image);
                        if (System.IO.File.Exists(oldImagePath)) {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploaDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imageName;

                }
               
                context.Update(product);
                await context.SaveChangesAsync();
                TempData["Success"] = "The product has been edited!";
                return RedirectToAction("Edit", new { id});
            }
            return View(product);
        }
        //GET /admin/product/delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["Error"] = "The product does not exist";
                // return NotFound();
            }
            else
            {
                if (!string.Equals(product.Image, "noimage.png"))
                {
                    string uploaDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
                    string oldImagePath = Path.Combine(uploaDir, product.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                TempData["Success"] = "The product has been deleted!";

            }
            return RedirectToAction("Index");
        }
    }
}
