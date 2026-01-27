namespace CityPointRoomHire.Models
{
    public class BookingEquipment
    {
            public int BookingEquipmentId { get; set; }
            public int BookingId { get; set; }
            public Booking Booking { get; set; }

            public int EquipmentId { get; set; }
            public Equipment Equipment { get; set; }

            public int Quantity { get; set; }
        



    }
}
