using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShopingCart.Models
{
    public class Page
    {
        public int Id { get; set; }
        [Required, MinLength(3, ErrorMessage ="Minimun length is 3")]
        public string Title { get; set; }
        public string Slug { get; set; }
        [Required, MinLength(3, ErrorMessage = "Minimun length is 3")]
        public string Content { get; set; }
        public int Sorting { get; set; }
    }
}
