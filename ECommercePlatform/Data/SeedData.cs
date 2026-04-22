using ECommercePlatform.Models; // Import the project's data models
using Microsoft.AspNetCore.Identity; // Import Identity services for users and roles
using Microsoft.EntityFrameworkCore; // Import Entity Framework Core for DB operations

namespace ECommercePlatform.Data // Define the namespace for data seeding logic
{ // Start of namespace
    public class SeedData // Define the SeedData utility class
    { // Start of class
        public static async Task SeedUsersAndRoles(IServiceProvider serviceProvider, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) // Method to seed roles and users
        { // Start of method
            // Seeded my roles // Developer comment
            string[] roleNames = { "Admin", "Supplier", "Standard", "Developer" }; // Define the array of roles to create
            foreach (string roleName in roleNames) // Iterate through each role name
            { // Start of loop
                var roleExists = await roleManager.RoleExistsAsync(roleName); // Check if the role already exists in the database
                if (!roleExists) // If the role does not exist
                { // Start if
                    var role = new IdentityRole(roleName); // Create a new IdentityRole object
                    await roleManager.CreateAsync(role); // Save the new role to the database
                } // End if
            } // End of loop

            // Seeding users and assigning roles, one for each type of user for now // Developer comment
            var adminUser = await userManager.FindByEmailAsync("admin@example.com"); // Try to find the admin user by email
            if (adminUser == null) // If the admin user is not found
            { // Start if
                adminUser = new IdentityUser { UserName = "admin@example.com", Email = "admin@example.com", EmailConfirmed = true }; // Create new user object
                await userManager.CreateAsync(adminUser, "Password123!"); // Create user with a specific password
            } // End if
            if (!await userManager.IsInRoleAsync(adminUser, "Admin")) // Check if user is already in the Admin role
            { // Start if
                await userManager.AddToRoleAsync(adminUser, "Admin"); // Assign the Admin role to the user
            } // End if

            var supplierUser = await userManager.FindByEmailAsync("supplier@example.com"); // Find the first supplier user
            if (supplierUser == null) // If not found
            { // Start if
                supplierUser = new IdentityUser { UserName = "supplier@example.com", Email = "supplier@example.com", EmailConfirmed = true }; // Initialize user object
                await userManager.CreateAsync(supplierUser, "Password123!"); // Save user to DB
            } // End if
            if (!await userManager.IsInRoleAsync(supplierUser, "Supplier")) // Check for Supplier role
            { // Start if
                await userManager.AddToRoleAsync(supplierUser, "Supplier"); // Assign Supplier role
            } // End if

            var supplierUser2 = await userManager.FindByEmailAsync("supplier2@example.com"); // Find the second supplier user
            if (supplierUser2 == null) // If not found
            { // Start if
                supplierUser2 = new IdentityUser { UserName = "supplier2@example.com", Email = "supplier2@example.com", EmailConfirmed = true }; // Initialize user object
                { // Extra block scope
                    await userManager.CreateAsync(supplierUser2, "Password123!"); // Save user to DB
                } // End scope
            } // End if
            if (!await userManager.IsInRoleAsync(supplierUser2, "Supplier")) // Check for Supplier role
            { // Start if
                await userManager.AddToRoleAsync(supplierUser2, "Supplier"); // Assign Supplier role
            } // End if

            var supplierUser3 = await userManager.FindByEmailAsync("supplier3@example.com"); // Find the third supplier user
            if (supplierUser3 == null) // If not found
            { // Start if
                supplierUser3 = new IdentityUser { UserName = "supplier3@example.com", Email = "supplier3@example.com", EmailConfirmed = true }; // Initialize user object
                await userManager.CreateAsync(supplierUser3, "Password123!"); // Save user to DB
            } // End if
            if (!await userManager.IsInRoleAsync(supplierUser3, "Supplier")) // Check for Supplier role
            { // Start if
                await userManager.AddToRoleAsync(supplierUser3, "Supplier"); // Assign Supplier role
            } // End if

            var devUser = await userManager.FindByEmailAsync("dev@example.com"); // Find the developer user
            if (devUser == null) // If not found
            { // Start if
                devUser = new IdentityUser { UserName = "dev@example.com", Email = "dev@example.com", EmailConfirmed = true }; // Initialize user object
                await userManager.CreateAsync(devUser, "Password123!"); // Save user to DB
            } // End if
            if (!await userManager.IsInRoleAsync(devUser, "Developer")) // Check for Developer role
            { // Start if
                await userManager.AddToRoleAsync(devUser, "Developer"); // Assign Developer role
            } // End if

            var normalUser = await userManager.FindByEmailAsync("user@example.com"); // Find the standard user
            if (normalUser == null) // If not found
            { // Start if
                normalUser = new IdentityUser { UserName = "user@example.com", Email = "user@example.com", EmailConfirmed = true }; // Initialize user object
                await userManager.CreateAsync(normalUser, "Password123!"); // Save user to DB
            } // End if
            if (!await userManager.IsInRoleAsync(normalUser, "Standard")) // Check for Standard role
            { // Start if
                await userManager.AddToRoleAsync(normalUser, "Standard"); // Assign Standard role
            } // End if
        } // End of SeedUsersAndRoles method

        public static async Task SeedSuppliers(IServiceProvider serviceProvider) // Method to seed the Suppliers table
        { // Start of method
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>(); // Get the UserManager service from the provider
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>(); // Get the database context service

            //Find the user by email // Developer comment
            var SupplierUser1 = await userManager.FindByEmailAsync("supplier@example.com"); // Retrieve user 1 for linking
            var SupplierUser2 = await userManager.FindByEmailAsync("supplier2@example.com"); // Retrieve user 2 for linking
            var SupplierUser3 = await userManager.FindByEmailAsync("supplier3@example.com"); // Retrieve user 3 for linking

            if (SupplierUser1 == null || SupplierUser2 == null || SupplierUser3 == null) // Check if any required user is missing
            { // Start if
                throw new Exception("Supplier user not found."); // Stop execution with an error message
            } // End if

            // prevent duplicate seeding // Developer comment
            if (context.Suppliers.Any()) // Check if the Suppliers table already contains data
                return; // Exit the method to avoid duplicates

            var suppliers = new List<Suppliers> // Initialize a list of new Supplier objects
            { // Start list
                new Suppliers // First supplier
                { // Start object
                    SupplierName = "Fresh Farm Produce", // Set name
                    SupplierEmail = "contac@freshfarm.co.uk", // Set contact email
                    SupplierInformation = "Local farm supplying organic fruits and vegetables.", // Set description
                    UserId = SupplierUser1.Id // Link to the Identity user ID
                }, // End object
                new Suppliers // Second supplier
                { // Start object
                    SupplierName ="UK Big Farm", // Set name
                    SupplierEmail = "sales@Bigfarm.co.uk", // Set contact email
                    SupplierInformation = "Biggest UK farm supplying organic fruits and vegetables.", // Set description
                    UserId = SupplierUser2.Id // Link to the Identity user ID
                }, // End object
                new Suppliers // Third supplier
                { // Start object
                    SupplierName ="Sandwell College Farm", // Set name
                    SupplierEmail = "farm@sandwellcollege.ac.uk", // Set contact email
                    SupplierInformation = "Local farm located in Sandwell supplying organic fruits and vegetables", // Set description
                    UserId = SupplierUser3.Id // Link to the Identity user ID
                } // End object
            }; // End list
            context.Suppliers.AddRange(suppliers); // Add the list to the context's Suppliers set
            await context.SaveChangesAsync(); // Commit the changes to the database
        } // End of SeedSuppliers method

        public static async Task SeedProducts(IServiceProvider serviceProvider) // Method to seed the Products table
        { // Start of method
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>(); // Resolve the database context
            // Find the supplier and you add as many as you need to // Developer comment
            var SandwellCollegeFarm = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierName == "Sandwell College Farm"); // Get supplier by name
            var UkBigFarm = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierName == "Uk Big Farm"); // Get supplier by name
            var FreshFarm = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierName == "Fresh Farm Produce"); // Get supplier by name

            if (SandwellCollegeFarm == null || UkBigFarm == null || FreshFarm == null) // Check if suppliers exist
            { // Start if
                throw new Exception("Supplier not found"); // Halt if foreign keys won't be valid
            } // End if

            if (!context.Products.Any()) // Only proceed if the Products table is empty
            { // Start if
                var products = new List<Products> // Initialize list of new product objects
                { // Start list
                    new Products // Apple product
                    { // Start object
                        ProductName = "Apple", // Set name
                        Stock = 200, // Set quantity
                        Price = 0.50m, // Set price (decimal)
                        SuppliersId = SandwellCollegeFarm.SuppliersId, // Link to supplier ID
                        ImagePath = "/image/apple.jpg" // Set image URL
                    }, // End object
                    new Products // Bread product
                    { // Start object
                        ProductName = "Bread", // Set name
                        Stock = 130, // Set quantity
                        Price = 0.20m, // Set price
                        SuppliersId = UkBigFarm.SuppliersId, // Link to supplier ID
                        ImagePath = "/image/Bread.jpg" // Set image URL
                    }, // End object
                     new Products // Milk product
                     { // Start object
                        ProductName = "Milk", // Set name
                        Stock = 100, // Set quantity
                        Price = 0.40m, // Set price
                        SuppliersId = FreshFarm.SuppliersId, // Link to supplier ID
                        ImagePath = "/image/milk.jpg" // Set image URL
                     }, // End object
                      new Products // Rice product
                      { // Start object
                        ProductName = "Rice", // Set name
                        Stock = 150, // Set quantity
                        Price = 1.20m, // Set price
                        SuppliersId = UkBigFarm.SuppliersId, // Link to supplier ID
                        ImagePath = "/image/rice.jpg" // Set image URL
                      }, // End object
                       new Products // Carrot product
                       { // Start object
                        ProductName = "Carrot", // Set name
                        Stock = 150, // Set quantity
                        Price = 1.20m, // Set price
                        SuppliersId = UkBigFarm.SuppliersId, // Link to supplier ID
                        ImagePath = "/image/Carrot.jpg" // Set image URL
                       }, // End object
                        new Products // Grapes product
                        { // Start object
                        ProductName = "Grapes", // Set name
                        Stock = 100, // Set quantity
                        Price = 0.40m, // Set price
                        SuppliersId = FreshFarm.SuppliersId, // Link to supplier ID
                        ImagePath = "/image/grapes.jpg" // Set image URL
                        }, // End object
                        new Products // Corn product
                        { // Start object
                        ProductName = "Corn", // Set name
                        Stock = 200, // Set quantity
                        Price = 0.50m, // Set price
                        SuppliersId = SandwellCollegeFarm.SuppliersId, // Link to supplier ID
                        ImagePath = "/image/corn.jpg" // Set image URL
                        }, // End object
                        new Products // Beans product
                        { // Start object
                        ProductName = "Beans", // Set name
                        Stock = 200, // Set quantity
                        Price = 0.50m, // Set price
                        SuppliersId = SandwellCollegeFarm.SuppliersId, // Link to supplier ID
                        ImagePath = "/image/beans.jpg" // Set image URL
                        }, // End object
                         new Products // Egg product
                         { // Start object
                        ProductName = "Egg", // Set name
                        Stock = 150, // Set quantity
                        Price = 1.20m, // Set price
                        SuppliersId = UkBigFarm.SuppliersId, // Link to supplier ID
                        ImagePath = "/image/egg.jpg" // Set image URL
                        }, // End object
                         new Products // Strawberry product
                       { // Start object
                        ProductName = "Strawberry", // Set name
                        Stock = 150, // Set quantity
                        Price = 1.20m, // Set price
                        SuppliersId = UkBigFarm.SuppliersId, // Link to supplier ID
                        ImagePath = "/image/strawberry.jpg" // Set image URL
                        }, // End object
                         new Products // Tomato product
                         { // Start object
                         ProductName = "Tomatoe", // Set name
                         Stock = 150, // Set quantity
                         Price = 1.20m, // Set price
                         SuppliersId = UkBigFarm.SuppliersId, // Link to supplier ID
                         ImagePath = "/image/tomatoe.jpg" // Set image URL
                         }, // End object
                          new Products // Meat product
                     { // Start object
                        ProductName = "Meat", // Set name
                        Stock = 100, // Set quantity
                        Price = 0.40m, // Set price
                        SuppliersId = FreshFarm.SuppliersId, // Link to supplier ID
                        ImagePath = "/image/meat.jpg" // Set image URL
                     }, // End object
                         new Products // Honey product
                     { // Start object
                        ProductName = "Honey", // Set name
                        Stock = 100, // Set quantity
                        Price = 0.40m, // Set price
                        SuppliersId = FreshFarm.SuppliersId, // Link to supplier ID
                        ImagePath = "/image/honey.jpg" // Set image URL
                     }, // End object
                }; // End list
                await context.Products.AddRangeAsync(products); // Add all products to the context asynchronously
                await context.SaveChangesAsync(); // Commit the products to the database
            } // End if

        } // End of SeedProducts method
    } // End of class
} // End of namespace