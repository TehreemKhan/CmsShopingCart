﻿using System.ComponentModel.DataAnnotations;

namespace CmsShopingCart.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}


