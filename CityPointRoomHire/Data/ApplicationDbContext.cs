using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CityPointRoomHire.Models;

namespace CityPointRoomHire.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CityPointRoomHire.Models.Booking> Booking { get; set; } = default!;
        public DbSet<CityPointRoomHire.Models.Customer> Customer { get; set; } = default!;
        public DbSet<CityPointRoomHire.Models.Equipment> Equipment { get; set; } = default!;
        public DbSet<CityPointRoomHire.Models.BookingEquipment> BookingEquipment { get; set; } = default!;
        public DbSet<CityPointRoomHire.Models.Staff> Staff { get; set; } = default!;
        public DbSet<CityPointRoomHire.Models.Room> Room { get; set; } = default!;
    }
}
