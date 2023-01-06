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
    [Authorize(Roles = "admin, editor")]
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly CmsShopingCartContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public BlogController(CmsShopingCartContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }
        //GET /admin/blog
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6;
            var blogs = context.Blogs.OrderByDescending(x => x.Id)
                                            .Skip((p-1)* pageSize)
                                            .Take(pageSize);
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Products.Count() / pageSize);
            return View(await blogs.ToListAsync());
        }
        //GET /admin/blog/details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Blog blog = await context.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }
        //GET /admin/blog/create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //POST /admin/blog/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
         
            if (ModelState.IsValid)
            {
                blog.Slug = blog.Title.ToLower().Replace(" ", "-");
                

                var slug = await context.Blogs.FirstOrDefaultAsync(x => x.Slug == blog.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The blog already exist");
                    return View(blog);
                }
                //upload image
                string imageName = "noimage.png";
                if (blog.ImageUpload != null) {
                    string uploaDir = Path.Combine(webHostEnvironment.WebRootPath, "media/blogs");
                    imageName = Guid.NewGuid().ToString() + "_" + blog.ImageUpload.FileName;
                    string filePath = Path.Combine(uploaDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await blog.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    
                }
                blog.Image = imageName;
                context.Add(blog);
                await context.SaveChangesAsync();
                TempData["Success"] = "The blog has been added!";
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        //GET /admin/blog/edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        { 
            Blog blog = await context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
          
            return View(blog);
        }
        //POST /admin/blog/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Blog blog)
        {
          
            if (ModelState.IsValid)
            {
                blog.Slug = blog.Title.ToLower().Replace(" ", "-");
                var slug = await context.Blogs.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == blog.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The blog already exist");
                    return View(blog);
                }
                //upload image
       
                if (blog.ImageUpload != null)
                {
                    string uploaDir = Path.Combine(webHostEnvironment.WebRootPath, "media/blogs");
                    if (!string.Equals(blog.Image, "noimage.png")) {
                        string oldImagePath = Path.Combine(uploaDir, blog.Image);
                        if (System.IO.File.Exists(oldImagePath)) {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    string imageName = Guid.NewGuid().ToString() + "_" + blog.ImageUpload.FileName;
                    string filePath = Path.Combine(uploaDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await blog.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    blog.Image = imageName;

                }
               
                context.Update(blog);
                await context.SaveChangesAsync();
                TempData["Success"] = "The blog has been edited!";
                return RedirectToAction("Edit", new { id});
            }
            return View(blog);
        }
        //GET /admin/blog/delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Blog blog = await context.Blogs.FindAsync(id);
            if (blog == null)
            {
                TempData["Error"] = "The blog does not exist";
                
            }
            else
            {
                if (!string.Equals(blog.Image, "noimage.png"))
                {
                    string uploaDir = Path.Combine(webHostEnvironment.WebRootPath, "media/blogs");
                    string oldImagePath = Path.Combine(uploaDir, blog.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                context.Blogs.Remove(blog);
                await context.SaveChangesAsync();
                TempData["Success"] = "The blog has been deleted!";

            }
            return RedirectToAction("Index");
        }
    }
}
