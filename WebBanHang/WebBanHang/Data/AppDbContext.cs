using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;
namespace WebBanHang.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Order> Orders { get; set; }

        /*public AppDbContext(DbContextOptions options) : base(options)
        {
        }*/


    }
}
