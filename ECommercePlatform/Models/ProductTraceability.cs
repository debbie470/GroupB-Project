namespace ECommercePlatform.Models // Defines the organizational group for application data models
{ // Start of namespace block
    public class ProductTraceability // Entity class for tracking product origin, batch history, and certifications
    { // Start of class block
        public int ProductTraceabilityId { get; set; } // Unique identifier and primary key for the traceability record

        public int ProductsId { get; set; } // Foreign key property linking this record to a specific product

        public string Origin { get; set; } // Stores the geographical location or source where the product was produced

        public string BatchNumber { get; set; } // Unique code assigned to a specific production or harvest run

        public DateOnly HarvestDate { get; set; } // Records the specific calendar date the product was harvested or manufactured

        public string Certifications { get; set; } // Lists quality standards, organic labels, or legal certifications held by the product

        public Products Products { get; set; } // Navigation property to access details of the associated product entity
    } // End of class block
} // End of namespace block