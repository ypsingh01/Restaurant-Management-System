using RestaurantAPI.DTOs;

namespace RestaurantAPI.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string? Error, AuthResponseDTO? Response)> LoginAsync(LoginRequestDTO dto);
        Task<(bool Success, string? Error, AuthResponseDTO? Response)> RegisterAsync(RegisterRequestDTO dto);
    }
}
