using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShopingCart.Models
{
    public class Login
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password),Required, MinLength(6, ErrorMessage = "Minimun length is 6")]
        public string Password { get; set; }

        public string RetrnUrl { get; set; }

    }
}

