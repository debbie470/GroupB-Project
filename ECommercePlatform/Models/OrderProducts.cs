namespace ECommercePlatform.Models // Defines the organizational group for application data models
{ // Start of namespace block
    public class OrderProducts // Represents the junction entity between products and finalized orders
    { // Start of class block
        public int OrderProductsId { get; set; } // Unique identifier and primary key for the order-product record

        public int ProductsId { get; set; } // Foreign key property that links the entry to a specific product

        public int OrdersId { get; set; } // Foreign key property that links the entry to a specific order 

        public int Quantity { get; set; } // Stores the specific number of units purchased for this order line item 

        public Products Products { get; set; } // Navigation property to access the details of the associated product

        public Orders Orders { get; set; } // Navigation property to access the details of the associated order
    } // End of class block
} // End of namespace block