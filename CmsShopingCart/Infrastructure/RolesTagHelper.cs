using CmsShopingCart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShopingCart.Infrastructure
{
    [HtmlTargetElement("td", Attributes = "user-role")]
    public class RolesTagHelper : TagHelper
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        public RolesTagHelper(RoleManager<IdentityRole> _roleManager, UserManager<AppUser> _userManager)
        {
            this.roleManager = _roleManager;
            this.userManager = _userManager;
        }
        [HtmlAttributeName("user-role")]
        public string RoleId { get; set; }

        public override async Task ProcessAsync(TagHelperContext conext, TagHelperOutput output) {
            List<string> names = new List<string>();
            IdentityRole role = await roleManager.FindByIdAsync(RoleId);
            if (role != null) {
                foreach (var user in userManager.Users) {
                    if (user != null && await userManager.IsInRoleAsync(user, role.Name)) {

                        names.Add(user.UserName);
                    }
                }
            }
            output.Content.SetContent(names.Count == 0 ? "No users": string.Join(", ",names));
        }
    }
}
