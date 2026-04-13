namespace ECommercePlatform.Models
{
    public class DeliveryInfo
    {
        public int DeliveryInfoId { get; set; }
        public int OrdersId { get; set; }
        public string DeliveryType { get; set; }
        public DateTime ScheduledDateTime { get; set; } = DateTime.UtcNow;
        public string Status { get; set; }
        public Orders Orders { get; set; }
    }
}
