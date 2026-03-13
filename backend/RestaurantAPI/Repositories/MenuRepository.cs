using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using RestaurantAPI.Models;

namespace RestaurantAPI.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly RestaurantDbContext _context;

        public MenuRepository(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            return await _context.MenuItems.AsNoTracking().ToListAsync();
        }

        public async Task<MenuItem?> GetByIdAsync(int id)
        {
            return await _context.MenuItems.FindAsync(id);
        }

        public async Task<MenuItem> AddAsync(MenuItem item)
        {
            _context.MenuItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task UpdateAsync(MenuItem item)
        {
            _context.MenuItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(MenuItem item)
        {
            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.MenuItems.AnyAsync(mi => mi.MenuItemId == id);
        }
    }
}

