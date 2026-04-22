namespace ECommercePlatform.Models // Defines the organizational group for application data models
{ // Start of namespace block
    public class Products // Defines the entity for merchandise sold on the platform
    { // Start of class block
        public int ProductsId { get; set; } // Unique identifier and primary key for the product

        public int SuppliersId { get; set; } // Foreign key linking this product to its provider

        public string ProductName { get; set; } // Stores the descriptive name of the item

        public int Stock { get; set; } // Current quantity of the item available in inventory

        public decimal Price { get; set; } // Unit cost of the product for customers

        public string? ImagePath { get; set; } // Optional relative path or URL for the product's display image

        public Suppliers Suppliers { get; set; } // Navigation property to access the linked Supplier entity

        public ICollection<OrderProducts>? OrderProducts { get; set; } // Collection of relationship records for finalized orders

        public ICollection<BasketProducts>? BasketProducts { get; set; } // Collection of relationship records for items currently in user baskets
    } // End of class block
} // End of namespace block