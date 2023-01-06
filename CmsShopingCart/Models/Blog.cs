using CmsShopingCart.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShopingCart.Models
{
    public class Blog
    {
        public int Id { get; set; }
        [Required, MinLength(3, ErrorMessage = "Minimun length is 3.")]
        public string Title { get; set; }
        public string Slug { get; set; }
        [Required, MinLength(3, ErrorMessage = "Minimun length is 3.")]
        public string Description { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Image { get; set; }
        [ForeignKey("CategoryId")]
       
        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }
    }
}
