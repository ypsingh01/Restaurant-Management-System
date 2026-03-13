namespace RestaurantAPI.DTOs
{
    public class CreateCustomerDTO
    {
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }

    public class CustomerDTO
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}

