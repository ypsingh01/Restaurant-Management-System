namespace RestaurantAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = null!; // "Admin" or "Customer"
        public int? CustomerId { get; set; }

        public Customer? Customer { get; set; }
    }
}
