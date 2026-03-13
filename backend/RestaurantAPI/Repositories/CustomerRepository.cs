using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using RestaurantAPI.Models;

namespace RestaurantAPI.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly RestaurantDbContext _context;

        public CustomerRepository(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(c => c.CustomerId == id);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.AsNoTracking().ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Customers.AnyAsync(c => c.CustomerId == id);
        }
    }
}

