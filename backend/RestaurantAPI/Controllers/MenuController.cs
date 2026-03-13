using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.DTOs;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItemDTO>>> GetAll()
        {
            var items = await _menuService.GetAllAsync();
            return Ok(items);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MenuItemDTO>> GetById(int id)
        {
            var item = await _menuService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<MenuItemDTO>> Create([FromBody] CreateMenuItemDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _menuService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.MenuItemId }, created);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateMenuItemDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _menuService.UpdateAsync(id, dto);
            if (!updated) return NotFound();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _menuService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}

