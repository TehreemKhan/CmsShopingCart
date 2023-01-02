using CmsShopingCart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShopingCart.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin, editor")]
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        public RolesController(RoleManager<IdentityRole> _roleManager, UserManager<AppUser> _userManager)
        {
            this.roleManager = _roleManager;
            this.userManager = _userManager;
        }
        //GET /admin/roles
        public IActionResult Index()
        {
            return View(roleManager.Roles);
        }

        //GET /admin/roles/create
        public IActionResult Create()
        {
            return View();
        }
        //POST /admin/roles/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([MinLength(2), Required] string name )
        {
            if (ModelState.IsValid) {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else {

                    foreach (IdentityError error in result.Errors) {

                         ModelState.AddModelError("", error.Description);
                    }
                }
            }
            ModelState.AddModelError("", "Minimum length is 2");
            return View();
        }
        //GET /admin/roles/edit/5
        [HttpGet]
        public async Task<IActionResult> Edit([MinLength(2), Required] string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();

            foreach (AppUser user in userManager.Users) {
                var list = await userManager.IsInRoleAsync(user, role.Name)? members : nonMembers;
                list.Add(user);
            }

            return View(new RoleEdit { 
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }
        //POST /admin/roles/edit
        [HttpPost]
        public async Task<IActionResult> Edit(RoleEdit roleEdit)
        {
            IdentityResult  result;
         
            foreach (string  userId in roleEdit.AddIds ?? new string[] { })
            {
                AppUser user = await userManager.FindByIdAsync(userId);
                result = await userManager.AddToRoleAsync(user, roleEdit.RoleName);
            }

            foreach (string userId in roleEdit.DeleteIds ?? new string[] { })
            {
                AppUser user = await userManager.FindByIdAsync(userId);
                result = await userManager.RemoveFromRoleAsync(user, roleEdit.RoleName);
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
