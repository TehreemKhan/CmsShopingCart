using CmsShopingCart.Infrastructure;
using CmsShopingCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShopingCart.Controllers
{
    public class PagesController : Controller
    {
        private readonly CmsShopingCartContext context;
        public PagesController(CmsShopingCartContext context)
        {
            this.context = context;
           
        }
        //GET or /slug
        public async  Task<IActionResult> Page(string slug)
        {
            if (slug == null) {
                return View(await context.Pages.Where(x => x.Slug == "home").FirstOrDefaultAsync());
            }
            Page page = await context.Pages.Where(x => x.Slug == slug).FirstOrDefaultAsync();
            if (page == null) {
                return NotFound();
            }
            return View(page);
        }
    }
}
