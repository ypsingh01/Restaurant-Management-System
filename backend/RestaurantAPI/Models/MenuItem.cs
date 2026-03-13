namespace RestaurantAPI.Models
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}

