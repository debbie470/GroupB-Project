namespace ECommercePlatform.Models // Defines the organizational group for application data models
{ // Start of namespace block
    public class Orders // Defines the entity class for managing customer orders and transactions
    { // Start of class block
        public int OrdersId { get; set; } // Unique identifier and primary key for the order record

        public string UserId { get; set; } // Property that links the order to a specific customer or user 

        public decimal Subtotal { get; set; } // Stores the total monetary value of the order after discounts

        public bool Delivery { get; set; } // Flag indicating if the customer chose the delivery option

        public bool Collection { get; set; } // Flag indicating if the customer chose to pick up the order

        public string? DeliveryType { get; set; } // Specifies the shipping method; nullable as it's not needed for collections 

        public string OrderTrackingStatus { get; set; } // Tracks the current fulfillment state (e.g., Pending, Shipped, Delivered)

        public DateOnly? CollectionDate { get; set; } // Stores the scheduled pickup date; nullable as it's not needed for deliveries 

        public DateOnly OrderDate { get; set; } // Records the calendar date when the order was placed

        public ICollection<OrderProducts>? OrderProducts { get; set; } // Navigation property representing the list of items included in this order
    } // End of class block
} // End of namespace block