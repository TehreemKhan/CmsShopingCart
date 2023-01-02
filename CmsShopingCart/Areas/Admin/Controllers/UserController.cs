using CmsShopingCart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShopingCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        public UserController(UserManager<AppUser> _userManager)
        {
            this.userManager = _userManager;
        }
        public IActionResult Index()
        {
            return View(userManager.Users);
        }
    }
}
