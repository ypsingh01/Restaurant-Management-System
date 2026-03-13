using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.DTOs
{
    public class LoginRequestDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required, MinLength(6)]
        public string Password { get; set; } = null!;
    }

    public class RegisterRequestDTO
    {
        [Required, MinLength(2)]
        public string Name { get; set; } = null!;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required, MinLength(6)]
        public string Password { get; set; } = null!;
        [Required, RegularExpression("^(Admin|Customer)$")]
        public string Role { get; set; } = null!;
        public string? Phone { get; set; }
    }

    public class AuthResponseDTO
    {
        public string Token { get; set; } = null!;
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int? CustomerId { get; set; }
    }
}
