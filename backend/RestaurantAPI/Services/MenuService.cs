using RestaurantAPI.DTOs;
using RestaurantAPI.Models;
using RestaurantAPI.Repositories;

namespace RestaurantAPI.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;

        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<IEnumerable<MenuItemDTO>> GetAllAsync()
        {
            var items = await _menuRepository.GetAllAsync();
            return items.Select(ToDto);
        }

        public async Task<MenuItemDTO?> GetByIdAsync(int id)
        {
            var item = await _menuRepository.GetByIdAsync(id);
            return item == null ? null : ToDto(item);
        }

        public async Task<MenuItemDTO> CreateAsync(CreateMenuItemDTO dto)
        {
            var entity = new MenuItem
            {
                Name = dto.Name,
                Category = dto.Category,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable
            };

            var created = await _menuRepository.AddAsync(entity);
            return ToDto(created);
        }

        public async Task<bool> UpdateAsync(int id, CreateMenuItemDTO dto)
        {
            var existing = await _menuRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Name = dto.Name;
            existing.Category = dto.Category;
            existing.Price = dto.Price;
            existing.IsAvailable = dto.IsAvailable;

            await _menuRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _menuRepository.GetByIdAsync(id);
            if (existing == null) return false;

            await _menuRepository.DeleteAsync(existing);
            return true;
        }

        private static MenuItemDTO ToDto(MenuItem item) => new()
        {
            MenuItemId = item.MenuItemId,
            Name = item.Name,
            Category = item.Category,
            Price = item.Price,
            IsAvailable = item.IsAvailable
        };
    }
}

