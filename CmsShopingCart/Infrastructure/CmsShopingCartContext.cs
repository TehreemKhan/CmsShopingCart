using CmsShopingCart.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShopingCart.Infrastructure
{
    public class CmsShopingCartContext :DbContext
    {
        public CmsShopingCartContext(DbContextOptions<CmsShopingCartContext> options):base(options)
        {

        }
        public DbSet<Page> Pages { get; set; }
    }
}
