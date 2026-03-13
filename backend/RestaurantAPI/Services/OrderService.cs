using RestaurantAPI.DTOs;
using RestaurantAPI.Models;
using RestaurantAPI.Repositories;

namespace RestaurantAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMenuRepository _menuRepository;

        private static readonly string[] AllowedStatuses =
            { "Pending", "Preparing", "Ready", "Delivered" };

        public OrderService(
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            IMenuRepository menuRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _menuRepository = menuRepository;
        }

        public async Task<OrderResponseDTO?> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order == null ? null : ToDto(order);
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(ToDto);
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetByCustomerIdAsync(int customerId)
        {
            var orders = await _orderRepository.GetByCustomerIdAsync(customerId);
            return orders.Select(ToDto);
        }

        public async Task<(bool Success, string? Error, OrderResponseDTO? Order)> CreateAsync(CreateOrderDTO dto)
        {
            var customerExists = await _customerRepository.ExistsAsync(dto.CustomerId);
            if (!customerExists)
                return (false, "Customer not found.", null);

            if (dto.Items == null || !dto.Items.Any())
                return (false, "Order must contain at least one item.", null);

            var menuItems = await _menuRepository.GetAllAsync();
            var menuDict = menuItems.ToDictionary(m => m.MenuItemId);

            var order = new Order
            {
                CustomerId = dto.CustomerId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending"
            };

            decimal total = 0;

            foreach (var dtoItem in dto.Items)
            {
                if (!menuDict.TryGetValue(dtoItem.MenuItemId, out var menuItem))
                    return (false, $"Menu item {dtoItem.MenuItemId} not found.", null);

                if (!menuItem.IsAvailable)
                    return (false, $"Menu item {menuItem.Name} is not available.", null);

                if (dtoItem.Quantity <= 0)
                    return (false, "Quantity must be greater than zero.", null);

                var price = menuItem.Price;
                total += price * dtoItem.Quantity;

                order.OrderItems.Add(new OrderItem
                {
                    MenuItemId = dtoItem.MenuItemId,
                    Quantity = dtoItem.Quantity,
                    Price = price
                });
            }

            order.TotalAmount = total;

            var created = await _orderRepository.AddAsync(order);
            var loaded = await _orderRepository.GetByIdAsync(created.OrderId)
                         ?? created;

            return (true, null, ToDto(loaded));
        }

        public async Task<(bool Success, string? Error)> UpdateStatusAsync(int orderId, string status)
        {
            if (!AllowedStatuses.Contains(status))
                return (false, "Invalid status value.");

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                return (false, "Order not found.");

            order.Status = status;
            await _orderRepository.UpdateAsync(order);
            return (true, null);
        }

        private static OrderResponseDTO ToDto(Order order)
        {
            return new OrderResponseDTO
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(oi => new OrderItemResponseDTO
                {
                    OrderItemId = oi.OrderItemId,
                    MenuItemId = oi.MenuItemId,
                    MenuItemName = oi.MenuItem?.Name ?? string.Empty,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            };
        }
    }
}

