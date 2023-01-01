using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShopingCart.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required, MinLength(3, ErrorMessage = "Minimun length is 3.")]
        [RegularExpression(@"^[a-zA-Z-]+$", ErrorMessage = "Only letters are allowed.")]
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Sorting { get; set; }
    }
}
