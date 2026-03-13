using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.DTOs;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login([FromBody] LoginRequestDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var (success, error, response) = await _authService.LoginAsync(dto);
            if (!success) return BadRequest(new { message = error });
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register([FromBody] RegisterRequestDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var (success, error, response) = await _authService.RegisterAsync(dto);
            if (!success) return BadRequest(new { message = error });
            return Ok(response);
        }
    }
}
