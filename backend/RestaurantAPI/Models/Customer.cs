namespace RestaurantAPI.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}

