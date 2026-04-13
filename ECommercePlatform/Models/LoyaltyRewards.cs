namespace ECommercePlatform.Models
{
    public class LoyaltyRewards
    {
        public int LoyaltyRewardsId { get; set; }
        public string UserId { get; set; }
        public int PointsBalance { get; set; }
        public string TierLevel { get; set; }
        public string History { get; set; }
    }
}
