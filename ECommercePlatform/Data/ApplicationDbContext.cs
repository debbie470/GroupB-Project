using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ECommercePlatform.Models;

namespace ECommercePlatform.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ECommercePlatform.Models.Suppliers> Suppliers { get; set; } = default!;
        public DbSet<ECommercePlatform.Models.ProductTraceability> ProductTraceability { get; set; } = default!;
        public DbSet<ECommercePlatform.Models.Products> Products { get; set; } = default!;
        public DbSet<ECommercePlatform.Models.Orders> Orders { get; set; } = default!;
        public DbSet<ECommercePlatform.Models.OrderProducts> OrderProducts { get; set; } = default!;
        public DbSet<ECommercePlatform.Models.LoyaltyRewards> LoyaltyRewards { get; set; } = default!;
        public DbSet<ECommercePlatform.Models.DeliveryInfo> DeliveryInfo { get; set; } = default!;
        public DbSet<ECommercePlatform.Models.BasketProducts> BasketProducts { get; set; } = default!;
        public DbSet<ECommercePlatform.Models.Basket> Basket { get; set; } = default!;
    }
}
