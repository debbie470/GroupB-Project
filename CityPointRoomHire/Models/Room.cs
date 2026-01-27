namespace CityPointRoomHire.Models
{
    public class Room
    {
        
            public int RoomId { get; set; }
            public string RoomName { get; set; }
            public int Capacity { get; set; }
            public bool HasProjector { get; set; }
            public bool WheelchairAccessible { get; set; }
            public int PricePerHour { get; set; }

            public ICollection<Booking> Bookings { get; set; }
        

    }
}
