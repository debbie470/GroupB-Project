namespace ECommerceCore.Models
{
    public class Orders
    {
        public int OrdersId { get; set; }
        public string UserId { get; set; } // Links the order to a user 
        public decimal Subtotal { get; set; }
        public bool Delivery { get; set; }
        public bool Collection { get; set; }
        public string? DeliveryType { get; set; } //This has to be nullable as they may pick collection 
        public string OrderTrackingStatus { get; set; }
        public DateOnly? CollectionDate { get; set; } // same here needs to be nullable as they may pick delivery 
        public DateOnly OrderDate { get; set; }

        public ICollection<OrderProducts>? OrderProducts { get; set; }
    }
}
