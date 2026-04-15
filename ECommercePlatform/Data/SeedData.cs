using ECommercePlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Data
{
    public class SeedData
    {
        public static async Task SeedUsersAndRoles(IServiceProvider serviceProvider, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seeded my roles
            string[] roleNames = { "Admin", "Supplier", "Standard", "Developer" };
            foreach (string roleName in roleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    var role = new IdentityRole(roleName);
                    await roleManager.CreateAsync(role);
                }
            }
            // Seeding users and assigning roles, one for each type of user for now 
            var adminUser = await userManager.FindByEmailAsync("admin@example.com");
            if (adminUser == null)
            {

                adminUser = new IdentityUser { UserName = "admin@example.com", Email = "admin@example.com", EmailConfirmed = true };
                await userManager.CreateAsync(adminUser, "Password123!");
            }
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            var supplierUser = await userManager.FindByEmailAsync("supplier@example.com");
            if (supplierUser == null)
            {
                supplierUser = new IdentityUser { UserName = "supplier@example.com", Email = "supplier@example.com", EmailConfirmed = true };
                await userManager.CreateAsync(supplierUser, "Password123!");
            }
            if (!await userManager.IsInRoleAsync(supplierUser, "Supplier"))
            {
                await userManager.AddToRoleAsync(supplierUser, "Supplier");
            }
            var supplierUser2 = await userManager.FindByEmailAsync("supplier2@example.com");
            if (supplierUser2 == null)
            {
                supplierUser2 = new IdentityUser { UserName = "supplier2@example.com", Email = "supplier2@example.com", EmailConfirmed = true };
                {
                    await userManager.CreateAsync(supplierUser2, "Password123!");

                }
            }
            if (!await userManager.IsInRoleAsync(supplierUser2, "Supplier"))
            {
                await userManager.AddToRoleAsync(supplierUser2, "Supplier");
            }
            var supplierUser3 = await userManager.FindByEmailAsync("supplier3@example.com");
            if (supplierUser3 == null)
            {
                supplierUser3 = new IdentityUser { UserName = "supplier3@example.com", Email = "supplier3@example.com", EmailConfirmed = true };
                await userManager.CreateAsync(supplierUser3, "Password123!");
            }
            if (!await userManager.IsInRoleAsync(supplierUser3, "Supplier"))
            {
                await userManager.AddToRoleAsync(supplierUser3, "Supplier");
            }
            var devUser = await userManager.FindByEmailAsync("dev@example.com");
            if (devUser == null)
            {
                devUser = new IdentityUser { UserName = "dev@example.com", Email = "dev@example.com", EmailConfirmed = true };
                await userManager.CreateAsync(devUser, "Password123!");
            }
            if (!await userManager.IsInRoleAsync(devUser, "Developer"))
            {
                await userManager.AddToRoleAsync(devUser, "Developer");
            }
            var normalUser = await userManager.FindByEmailAsync("user@example.com");
            if (normalUser == null)
            {
                normalUser = new IdentityUser { UserName = "user@example.com", Email = "user@example.com", EmailConfirmed = true };
                await userManager.CreateAsync(normalUser, "Password123!");
            }
            if (!await userManager.IsInRoleAsync(normalUser, "Standard"))
            {
                await userManager.AddToRoleAsync(normalUser, "Standard");
            }
        }
        public static async Task SeedSuppliers(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            //Find the user by email
            var SupplierUser1 = await userManager.FindByEmailAsync("supplier@example.com");
            var SupplierUser2 = await userManager.FindByEmailAsync("supplier2@example.com");
            var SupplierUser3 = await userManager.FindByEmailAsync("supplier3@example.com");

            if (SupplierUser1 == null || SupplierUser2 == null || SupplierUser3 == null)
            {
                throw new Exception("Supplier user not found.");
            }
            // prevent duplicate seeding 
            if (context.Suppliers.Any())
                return;
            var suppliers = new List<Suppliers>
            {
                new Suppliers
                {
                    SupplierName = "Fresh Farm Produce",
                    SupplierEmail = "contac@freshfarm.co.uk",
                    SupplierInformation = "Local farm supplying organic fruits and vegetables.",
                    UserId = SupplierUser1.Id
                },
                new Suppliers
                {
                    SupplierName ="UK Big Farm",
                    SupplierEmail = "sales@Bigfarm.co.uk",
                    SupplierInformation = "Biggest UK farm supplying organic fruits and vegetables.",
                    UserId = SupplierUser2.Id
                },
                new Suppliers
                {
                    SupplierName ="Sandwell College Farm",
                    SupplierEmail = "farm@sandwellcollege.ac.uk",
                    SupplierInformation = "Local farm located in Sandwell supplying organic fruits and vegetables",
                    UserId = SupplierUser3.Id
                }
            };
            context.Suppliers.AddRange(suppliers);
            await context.SaveChangesAsync();
        }
        public static async Task SeedProducts(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            // Find the supplier and you add as many as you need to
            var SandwellCollegeFarm = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierName == "Sandwell College Farm");
            var UkBigFarm = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierName == "Uk Big Farm");
            var FreshFarm = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierName == "Fresh Farm Produce");

            if (SandwellCollegeFarm == null || UkBigFarm == null || FreshFarm == null)
            {
                throw new Exception("Supplier not found");
            }
            if (!context.Products.Any())
            {
                var products = new List<Products>
                {
                    new Products
                    {
                        ProductName = "Apple",
                        Stock = 200,
                        Price = 0.50m,
                        SuppliersId = SandwellCollegeFarm.SuppliersId,
                        ImagePath = "/image/apple.jpg"
                    },
                    new Products
                    {
                        ProductName = "Bread",
                        Stock = 130,
                        Price = 0.20m,
                        SuppliersId = UkBigFarm.SuppliersId,
                        ImagePath = "/image/Bread.jpg"
                    },
                     new Products
                     {
                        ProductName = "Milk",
                        Stock = 100,
                        Price = 0.40m,
                        SuppliersId = FreshFarm.SuppliersId,
                        ImagePath = "/image/milk.jpg"
                     },
                      new Products
                      {
                        ProductName = "Rice",
                        Stock = 150,
                        Price = 1.20m,
                        SuppliersId = UkBigFarm.SuppliersId,
                        ImagePath = "/image/rice.jpg"
                      },
                };
                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }

        }
    }
}


    
