namespace MockExams.Models
{
    public class Room
    {
        
        
            public int RoomId { get; set; }

            public string RoomName { get; set; }
            public int Capacity { get; set; }
            public decimal PricePerHour { get; set; }

            public string Description { get; set; }
            public bool IsAvailable { get; set; }

            public ICollection<Booking>? Bookings { get; set; }
       

    }
}
