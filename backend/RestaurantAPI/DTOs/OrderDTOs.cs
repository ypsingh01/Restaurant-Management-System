namespace RestaurantAPI.DTOs
{
    public class CreateOrderItemDTO
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateOrderDTO
    {
        public int CustomerId { get; set; }
        public List<CreateOrderItemDTO> Items { get; set; } = new();
    }

    public class OrderItemResponseDTO
    {
        public int OrderItemId { get; set; }
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderResponseDTO
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public List<OrderItemResponseDTO> Items { get; set; } = new();
    }

    public class UpdateOrderStatusDTO
    {
        public string Status { get; set; } = null!;
    }
}

