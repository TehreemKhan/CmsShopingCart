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
    public class CartController : Controller
    {
        private readonly CmsShopingCartContext context;
        public CartController(CmsShopingCartContext context)
        {
            this.context = context;
        }
        //GET /cart
        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            CartViewModel CartVM = new CartViewModel { 
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)
            };
            return View(CartVM);
        }
        //GET /cart/add/5
        public async Task<IActionResult> Add(int id)
        {
            Product product = await context.Products.FindAsync(id);
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();
            if (cartItem == null) {
                cart.Add(new CartItem(product));
            } else {
                cartItem.Quantity += 1;
            }
            HttpContext.Session.SetJson("Cart",cart);
            return RedirectToAction("Index");
        }
    }
}
