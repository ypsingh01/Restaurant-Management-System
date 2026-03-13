using RestaurantAPI.Models;

namespace RestaurantAPI.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> AddAsync(Customer customer);
        Task<Customer?> GetByIdAsync(int id);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<bool> ExistsAsync(int id);
    }
}

