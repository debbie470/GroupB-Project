namespace MockExams.Models
{
    public class Equipment
    {
        
            public int EquipmentId { get; set; }

            public string Name { get; set; }
            public string Description { get; set; }

            public int QuantityAvailable { get; set; }
            public decimal Price { get; set; }
            public bool IsAvailable { get; set; }

            // Equipment can appear in many equipment bookings
            public ICollection<BookingEquipment>? BookingEquipment { get; set; }
        
    
    }
}
