namespace ECommercePlatform.Models // Defines the organizational group for application data models
{ // Start of namespace block
    public class Suppliers // Defines the entity class for businesses or individuals providing products
    { // Start of class block
        public int SuppliersId { get; set; } // Unique identifier and primary key for the supplier record

        public string UserId { get; set; } // Property linking the supplier profile to a specific system user account

        public string SupplierName { get; set; } // Stores the official name of the supplier or company

        public string SupplierEmail { get; set; } // Stores the primary contact email address for the supplier

        public string SupplierInformation { get; set; } // Detailed description or metadata about the supplier's services and background

        public ICollection<Products>? Products { get; set; } // Navigation property representing the collection of products managed by this supplier
    } // End of class block
} // End of namespace block