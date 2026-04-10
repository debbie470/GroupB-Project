using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ECommerceCore.Models;

namespace ECommerceCore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ECommerceCore.Models.Suppliers> Suppliers { get; set; } = default!;
        public DbSet<ECommerceCore.Models.ProductTraceability> ProductTraceability { get; set; } = default!;
        public DbSet<ECommerceCore.Models.Products> Products { get; set; } = default!;
        public DbSet<ECommerceCore.Models.Orders> Orders { get; set; } = default!;
        public DbSet<ECommerceCore.Models.OrderProducts> OrderProducts { get; set; } = default!;
        public DbSet<ECommerceCore.Models.LoyaltyRewards> LoyaltyRewards { get; set; } = default!;
        public DbSet<ECommerceCore.Models.DeliveryInfo> DeliveryInfo { get; set; } = default!;
        public DbSet<ECommerceCore.Models.BasketProducts> BasketProducts { get; set; } = default!;
        public DbSet<ECommerceCore.Models.Basket> Basket { get; set; } = default!;
    }
}
