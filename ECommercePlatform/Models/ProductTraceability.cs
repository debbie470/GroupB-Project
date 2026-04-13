namespace ECommercePlatform.Models
{
    public class ProductTraceability
    {
        public int ProductTraceabilityId { get; set; }
        public int ProductsId { get; set; }
        public string Origin { get; set; }
        public string BatchNumber { get; set; }
        public DateOnly HarvestDate { get; set; }
        public string Certifications { get; set; }
        public Products Products { get; set; }
    }
}
