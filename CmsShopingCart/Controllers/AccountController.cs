using CmsShopingCart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShopingCart.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IPasswordHasher<AppUser> passwordHasher;
        public AccountController(UserManager<AppUser> _userManager,
                                SignInManager<AppUser> _signInManager,
                                IPasswordHasher<AppUser> _passwordHasher)
        {
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.passwordHasher = _passwordHasher;
        }
        //GET /account/register
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        //Post /account/register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {
                    UserName = user.UserName,
                    Email = user.Email,
                };
                IdentityResult result = await userManager.CreateAsync(appUser, user.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else {
                    foreach (IdentityError error in  result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(user);
        }
        //GET /account/login
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            Login login = new Login
            {
                RetrnUrl = returnUrl
            };
            return View(login);
        }
        //Post /account/login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await userManager.FindByEmailAsync(login.Email);
               
                if (appUser != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUser, login.Password, false, false);

                    if (result.Succeeded) {
                        return Redirect(login.RetrnUrl ?? "/");
                    }
                }
                ModelState.AddModelError("","Login failed  wrong credaintials");
            }
            return View(login);
        }
        //GET /account/logout
      
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Redirect("/");
        }
        //GET /account/edit
        public async Task<IActionResult> Edit()
        {
            AppUser appUser = await userManager.FindByNameAsync(User.Identity.Name);
            UserEdit user = new UserEdit(appUser);
            return View(user);
        }
        //Post /account/edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEdit user)
        {
            AppUser appUser = await userManager.FindByNameAsync(User.Identity.Name);
            if (ModelState.IsValid)
            {
                appUser.Email = user.Email;
                if (user.Password != null) {
                    appUser.PasswordHash = passwordHasher.HashPassword(appUser, user.Password);
                }
                IdentityResult result = await userManager.UpdateAsync(appUser);
                if (result.Succeeded) {
                    TempData["Success"] = "your information ha been edited";
                    return Redirect("/");
                }
            }
            return View();
        }
    }
}
