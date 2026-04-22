namespace ECommercePlatform.Models // Defines the organizational group for application data models
{ // Start of namespace block
    public class LoyaltyRewards // Entity class representing user loyalty points and reward data
    { // Start of class block
        public int LoyaltyRewardsId { get; set; } // Unique identifier and primary key for the loyalty record

        public string UserId { get; set; } // Property that links these loyalty rewards to a specific user

        public int PointsBalance { get; set; } // Stores the current total of accumulated loyalty points

        public string TierLevel { get; set; } // Represents the user's status level (e.g., Bronze, Silver, Gold)

        public string History { get; set; } // Stores a log or description of past point transactions and updates
    } // End of class block
} // End of namespace block