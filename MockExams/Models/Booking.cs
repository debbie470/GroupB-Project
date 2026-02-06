namespace MockExams.Models
{
    public class Booking
    {
     
            public int BookingId { get; set; }


            // Room
            public int RoomId { get; set; }

      
            public DateTime StartDateTime { get; set; }
            public DateTime EndDateTime { get; set; }

            // pending / approved / denied / cancelled
            public string Status { get; set; }

            // Navigation
            
            public Room Room { get; set; }
          
       
     
    

    }
}
