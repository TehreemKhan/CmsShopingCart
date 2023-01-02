using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShopingCart.Models
{
    public class User
    {
        [Required, MinLength(3, ErrorMessage = "Minimun length is 3")]
        public string UserName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password),Required, MinLength(6, ErrorMessage = "Minimun length is 6")]
        public string Password { get; set; }
        public User()
        {
                
        }
        public User(AppUser appUser)
        {
            UserName = appUser.UserName;
            Email = appUser.Email;
            Password = appUser.PasswordHash;
        }
    }
}

