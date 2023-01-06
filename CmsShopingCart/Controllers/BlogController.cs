using CmsShopingCart.Infrastructure;
using CmsShopingCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShopingCart.Controllers
{
    public class BlogController : Controller
    {
        private readonly CmsShopingCartContext context;
       
        public BlogController(CmsShopingCartContext context)
        {
            this.context = context;
          
        }
        //GET /blog
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6;
            var blogs = context.Blogs.OrderByDescending(x => x.Id)
                                            .Skip((p - 1) * pageSize)
                                            .Take(pageSize);
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Blogs.Count() / pageSize);
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
    }
}
