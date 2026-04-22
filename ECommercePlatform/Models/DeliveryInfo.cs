namespace ECommercePlatform.Models // Defines the organizational group for application data models
{ // Start of namespace block
    public class DeliveryInfo // Defines the entity for tracking order delivery and scheduling details
    { // Start of class block
        public int DeliveryInfoId { get; set; } // Unique identifier and primary key for the delivery record

        public int OrdersId { get; set; } // Foreign key property linking this info to a specific order

        public string DeliveryType { get; set; } // Describes the method of delivery (e.g., Standard, Express, Collection)

        public DateTime ScheduledDateTime { get; set; } = DateTime.UtcNow; // The planned date and time for delivery, defaulting to current UTC time

        public string Status { get; set; } // Current state of the delivery (e.g., Pending, Out for Delivery, Completed)

        public Orders Orders { get; set; } // Navigation property to access the parent order's data
    } // End of class block
} // End of namespace block