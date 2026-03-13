using RestaurantAPI.Models;

namespace RestaurantAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> AddAsync(Order order);
        Task<Order?> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId);
        Task UpdateAsync(Order order);
    }
}

