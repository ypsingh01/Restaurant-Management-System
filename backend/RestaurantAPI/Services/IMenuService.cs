using RestaurantAPI.DTOs;

namespace RestaurantAPI.Services
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuItemDTO>> GetAllAsync();
        Task<MenuItemDTO?> GetByIdAsync(int id);
        Task<MenuItemDTO> CreateAsync(CreateMenuItemDTO dto);
        Task<bool> UpdateAsync(int id, CreateMenuItemDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}

