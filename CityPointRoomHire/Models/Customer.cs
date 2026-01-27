using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace CityPointRoomHire.Models
{
    public class Customer
    {
        
            public int CustomerId { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Role { get; set; } // Customer, Staff, Admin
            public bool AccessibilityNeeds { get; set; }

            public ICollection<Booking> Bookings { get; set; }
        

    }
}
