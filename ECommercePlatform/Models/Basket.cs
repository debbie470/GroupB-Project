namespace ECommercePlatform.Models //Defines the organizational group for application data models
{// Starts of namespace block
    public class Basket // Defines the shooping basket (cart) entity class
    {// Start of class block
        public int BasketId { get; set; }// Unique identifier and primary key for the basket 
        public bool Status { get; set; }// Flag to indicate if the basket is active (true) or checked ot (false)
        public DateTime BasketCreatedAt { get; set; } = DateTime.UtcNow; // Records the creation timestamp, defaulting to current UTC time
        public string UserId { get; set; } //Links each Basket to a User 
        public ICollection<BasketProducts>? BasketProducts { get; set; }// Navigation property representing the collection of products inside this basket
    }//End of class block 
}//End of namespace block
