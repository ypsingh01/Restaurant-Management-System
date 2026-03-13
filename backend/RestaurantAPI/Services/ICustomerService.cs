using RestaurantAPI.DTOs;

namespace RestaurantAPI.Services
{
    public interface ICustomerService
    {
        Task<CustomerDTO> CreateAsync(CreateCustomerDTO dto);
        Task<IEnumerable<CustomerDTO>> GetAllAsync();
        Task<IEnumerable<OrderResponseDTO>> GetOrdersAsync(int customerId);
    }
}

