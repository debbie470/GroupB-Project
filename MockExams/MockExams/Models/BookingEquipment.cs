namespace MockExams.Models
{
    public class BookingEquipment
    {
       
            public int BookingEquipmentId { get; set; }

           
            public int EquipmentId { get; set; }

            public int Quantity { get; set; }
        public bool IsAvailable { get; set; }

        // Navigation

        public Equipment Equipment { get; set; }
            
        

    }
}
