using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Provides the base class for Identity-integrated database contexts
using Microsoft.EntityFrameworkCore; // Provides core Entity Framework functionality for database interactions
using ECommercePlatform.Models; // Grants access to the domain models defined in the project

namespace ECommercePlatform.Data // Defines the namespace for data-related logic
{ // Start of the namespace block
    public class ApplicationDbContext : IdentityDbContext // Defines the main database context class, inheriting Identity support
    { // Start of the class block
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) // Constructor that accepts configuration options
            : base(options) // Passes the configuration options to the base IdentityDbContext constructor
        { // Start of the constructor body
        } // End of the constructor body

        public DbSet<ECommercePlatform.Models.Suppliers> Suppliers { get; set; } = default!; // Represents the 'Suppliers' table in the database
        public DbSet<ECommercePlatform.Models.ProductTraceability> ProductTraceability { get; set; } = default!; // Represents the 'ProductTraceability' table
        public DbSet<ECommercePlatform.Models.Products> Products { get; set; } = default!; // Represents the 'Products' table
        public DbSet<ECommercePlatform.Models.Orders> Orders { get; set; } = default!; // Represents the 'Orders' table
        public DbSet<ECommercePlatform.Models.OrderProducts> OrderProducts { get; set; } = default!; // Represents the 'OrderProducts' table
        public DbSet<ECommercePlatform.Models.LoyaltyRewards> LoyaltyRewards { get; set; } = default!; // Represents the 'LoyaltyRewards' table
        public DbSet<ECommercePlatform.Models.DeliveryInfo> DeliveryInfo { get; set; } = default!; // Represents the 'DeliveryInfo' table
        public DbSet<ECommercePlatform.Models.BasketProducts> BasketProducts { get; set; } = default!; // Represents the 'BasketProducts' table
        public DbSet<ECommercePlatform.Models.Basket> Basket { get; set; } = default!; // Represents the 'Basket' table
    } // End of the class block
} // End of the namespace block