using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Data;
using RestaurantAPI.DTOs;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly RestaurantDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(RestaurantDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<(bool Success, string? Error, AuthResponseDTO? Response)> LoginAsync(LoginRequestDTO dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return (false, "Invalid email or password.", null);

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return (false, "Invalid email or password.", null);

            var token = GenerateJwt(user);
            return (true, null, new AuthResponseDTO
            {
                Token = token,
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                CustomerId = user.CustomerId
            });
        }

        public async Task<(bool Success, string? Error, AuthResponseDTO? Response)> RegisterAsync(RegisterRequestDTO dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return (false, "Email already registered.", null);

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password, BCrypt.Net.BCrypt.GenerateSalt(12));

            if (dto.Role == "Customer")
            {
                var customer = new Customer
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Phone = dto.Phone
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                var user = new User
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    PasswordHash = passwordHash,
                    Role = "Customer",
                    CustomerId = customer.CustomerId
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var token = GenerateJwt(user);
                return (true, null, new AuthResponseDTO
                {
                    Token = token,
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    CustomerId = user.CustomerId
                });
            }

            var adminUser = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = "Admin"
            };
            _context.Users.Add(adminUser);
            await _context.SaveChangesAsync();

            var adminToken = GenerateJwt(adminUser);
            return (true, null, new AuthResponseDTO
            {
                Token = adminToken,
                Id = adminUser.Id,
                Name = adminUser.Name,
                Email = adminUser.Email,
                Role = adminUser.Role,
                CustomerId = null
            });
        }

        private string GenerateJwt(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config["Jwt:Key"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("CustomerId", user.CustomerId?.ToString() ?? "")
            };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"] ?? "RestaurantAPI",
                audience: _config["Jwt:Audience"] ?? "RestaurantApp",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
