using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models;

namespace OnlineShop.Data
{
    //整合連接數據庫
    public class OnlineShopContext : DbContext
    {
        public OnlineShopContext (DbContextOptions<OnlineShopContext> options)
            : base(options)
        {
        }

        public DbSet<OnlineShop.Models.Product> Product { get; set; } 
        public DbSet<OnlineShop.Models.Category> Category { get; set; }
        public DbSet<OnlineShop.Models.Comment> Comment { get; set; }
        public DbSet<OnlineShop.Models.OrderItem> OrderItem { get; set; }
        public DbSet<OnlineShop.Models.Order> Order { get; set; }
    }
}
