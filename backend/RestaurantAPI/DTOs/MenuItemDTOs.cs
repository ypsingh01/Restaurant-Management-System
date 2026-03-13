namespace RestaurantAPI.DTOs
{
    public class CreateMenuItemDTO
    {
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    public class MenuItemDTO
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}

