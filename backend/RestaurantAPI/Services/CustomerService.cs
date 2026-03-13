using RestaurantAPI.DTOs;
using RestaurantAPI.Models;
using RestaurantAPI.Repositories;

namespace RestaurantAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;

        public CustomerService(
            ICustomerRepository customerRepository,
            IOrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }

        public async Task<CustomerDTO> CreateAsync(CreateCustomerDTO dto)
        {
            var entity = new Customer
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone
            };

            var created = await _customerRepository.AddAsync(entity);

            return new CustomerDTO
            {
                CustomerId = created.CustomerId,
                Name = created.Name,
                Email = created.Email,
                Phone = created.Phone
            };
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers.Select(c => new CustomerDTO
            {
                CustomerId = c.CustomerId,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone
            });
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetOrdersAsync(int customerId)
        {
            var orders = await _orderRepository.GetByCustomerIdAsync(customerId);
            return orders.Select(o => new OrderResponseDTO
            {
                OrderId = o.OrderId,
                CustomerId = o.CustomerId,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                Items = o.OrderItems.Select(oi => new OrderItemResponseDTO
                {
                    OrderItemId = oi.OrderItemId,
                    MenuItemId = oi.MenuItemId,
                    MenuItemName = oi.MenuItem?.Name ?? string.Empty,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            });
        }
    }
}

