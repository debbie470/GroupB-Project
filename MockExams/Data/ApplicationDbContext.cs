using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MockExams.Models;

namespace MockExams.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MockExams.Models.Booking> Booking { get; set; } = default!;
        public DbSet<MockExams.Models.BookingEquipment> BookingEquipment { get; set; } = default!;
        public DbSet<MockExams.Models.Equipment> Equipment { get; set; } = default!;
        public DbSet<MockExams.Models.Room> Room { get; set; } = default!;
        public DbSet<MockExams.Models.Staff> Staff { get; set; } = default!;
    }
}
