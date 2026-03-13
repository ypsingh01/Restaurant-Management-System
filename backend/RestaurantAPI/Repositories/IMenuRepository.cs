using RestaurantAPI.Models;

namespace RestaurantAPI.Repositories
{
    public interface IMenuRepository
    {
        Task<IEnumerable<MenuItem>> GetAllAsync();
        Task<MenuItem?> GetByIdAsync(int id);
        Task<MenuItem> AddAsync(MenuItem item);
        Task UpdateAsync(MenuItem item);
        Task DeleteAsync(MenuItem item);
        Task<bool> ExistsAsync(int id);
    }
}

