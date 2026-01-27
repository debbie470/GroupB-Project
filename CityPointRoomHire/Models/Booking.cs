using static CityPointRoomHire.Models.Customer;

namespace CityPointRoomHire.Models
{
    public class Booking
    {
        
        
            public int BookingId { get; set; }

            public int CustomerId { get; set; }
            public Customer Customer { get; set; }

            public int RoomId { get; set; }
            public Room Room { get; set; }

            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }

            public string Status { get; set; } // Pending, Approved, Rejected
            public DateTime CreatedAt { get; set; }
        

    }
}
