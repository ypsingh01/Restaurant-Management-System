using RestaurantAPI.DTOs;

namespace RestaurantAPI.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<OrderResponseDTO>> GetAllAsync();
        Task<IEnumerable<OrderResponseDTO>> GetByCustomerIdAsync(int customerId);
        Task<(bool Success, string? Error, OrderResponseDTO? Order)> CreateAsync(CreateOrderDTO dto);
        Task<(bool Success, string? Error)> UpdateStatusAsync(int orderId, string status);
    }
}

