using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MockExams.Models;

namespace MockExams.Data.Migrations
{
    public class SeedData
    {
        public static async Task SeedRoom(ApplicationDbContext context)
        {
            if (await context.Room.AnyAsync() == false)
            {
                var ListofRooms = new List<Room>
                {
                 new Room
                 {
                     RoomName = "Hotel1",
                     Capacity = 10,
                     PricePerHour = 10,
                     Description = "The room is medium-sized with white walls and a wooden floor. A large window lets in natural light, and the space feels clean and uncluttered.",
                     IsAvailable = true,


                 },
                 new Room
                 {
                     RoomName = "Hotel2",
                     Capacity = 20,
                     PricePerHour = 50,
                     Description = "The room has pale blue walls that reflect the afternoon light pouring in through a wide window. A wooden desk sits against one wall, scattered with books, while a neatly made bed and a small lamp give the space a quiet, personal feel.",
                     IsAvailable = true,
                 },
                 new Room
                 {
                     RoomName = "Hotel3",
                     Capacity = 2,
                     PricePerHour = 50,
                     Description = "The room has pale blue walls that reflect the afternoon light pouring in through a wide window. A wooden desk sits against one wall, scattered with books, while a neatly made bed and a small lamp give the space a quiet, personal feel.",
                     IsAvailable = true,
                 }

                };
                await context.Room.AddRangeAsync(ListofRooms);
                await context.SaveChangesAsync(); //save changes


            }

        }


        public static async Task SeedEquipment(ApplicationDbContext context)
        {
            if (await context.Equipment.AnyAsync() == false)
            {
                var ListofEquipments = new List<Equipment>
                {
                    new Equipment
                    {
                         Name = "Display Equipment",
                        Description = "Interactive Whiteboard",
                        QuantityAvailable = 3,
                        Price = 10,
                        IsAvailable = true,
                    },
                    new Equipment
                    {
                        Name = "Audio Equipment",
                        Description = "Wireless Microphones",
                        QuantityAvailable = 2,
                        Price = 20,
                        IsAvailable = true,
                    }

                };
                await context.Equipment.AddRangeAsync(ListofEquipments);
                await context.SaveChangesAsync();
            }
               


        }


        // New method to add user roles and
        public static async Task SeedUserRoles(IServiceProvider serviceProvider, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] arrayOfRoles = { "Admin", "Customer" };
            foreach (var role in arrayOfRoles)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if (roleExists == false)
                {
                    var FinalRole = new IdentityRole(role);
                    await roleManager.CreateAsync(FinalRole);
                }
            }

            // Creating Users
            var adminUser = await userManager.FindByEmailAsync("admin@admin.com"); // Checking if the user currently exists
            if (adminUser == null)
            {
                adminUser = new IdentityUser { UserName = "admin@admin.com", Email = "admin@admin.com", EmailConfirmed = true };
                await userManager.CreateAsync(adminUser, "Admin123!");
            }
            if (await userManager.IsInRoleAsync(adminUser, "Admin") == false)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");


                //Create A new user
                var customerUser = await userManager.FindByEmailAsync("customer@customer.com");
                if (customerUser == null)
                {
                    customerUser = new IdentityUser { UserName = "customer@customer.com", Email = "customer@customer.com", EmailConfirmed = true };
                    await userManager.CreateAsync(customerUser, "Customer123!");
                }
                if (await userManager.IsInRoleAsync(customerUser, "Customer") == false)
                {
                    await userManager.AddToRoleAsync(customerUser, "Customer");
                }

                //Create A new user 
                var staffUser = await userManager.FindByEmailAsync("staff@staff.com");
                if (staffUser == null)
                {
                    staffUser = new IdentityUser { UserName = "staff@staff.com", Email = "staff@staff.com", EmailConfirmed = true };
                    await userManager.CreateAsync(staffUser, "Staff123!");
                }
                if (await userManager.IsInRoleAsync(staffUser, "Staff") == false)
                {
                    await userManager.AddToRoleAsync(staffUser, "Staff");
                }
            }
        }
    }
}

       