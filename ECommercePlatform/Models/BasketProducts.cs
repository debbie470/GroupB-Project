namespace ECommercePlatform.Models // Defines the organizational group for application data models
{ // Start of namespace block
    public class BasketProducts // Represents the junction entity linking products to shopping baskets
    { // Start of class block
        public int BasketProductsId { get; set; } // Unique identifier and primary key for the record

        public int BasketId { get; set; } // Foreign key property that links to a specific shopping basket 

        public int ProductsId { get; set; } // Foreign key property that links to a specific product

        public int Quantity { get; set; } // Stores the number of units of this product added to the basket

        public Products Products { get; set; } // Navigation property to access the details of the linked product

        public Basket Basket { get; set; } // Navigation property to access the details of the linked basket
    } // End of class block
} // End of namespace block