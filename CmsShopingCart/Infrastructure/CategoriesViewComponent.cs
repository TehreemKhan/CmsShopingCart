using CmsShopingCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShopingCart.Infrastructure
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly CmsShopingCartContext context;
        public CategoriesViewComponent(CmsShopingCartContext _context)
        {
            this.context = _context;

        }
        public async Task<IViewComponentResult> InvokeAsync() {
            var categories = await GetPagesAsync();
            return View(categories);
        }
        private Task<List<Category>> GetPagesAsync() {
            return context.Categories.OrderBy(x => x.Sorting).ToListAsync();
        }
    }
}
