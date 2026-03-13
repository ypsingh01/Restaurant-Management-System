using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.DTOs;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponseDTO>> Create([FromBody] CreateOrderDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var (success, error, order) = await _orderService.CreateAsync(dto);
            if (!success) return BadRequest(new { message = error });

            return CreatedAtAction(nameof(GetById), new { id = order!.OrderId }, order);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderResponseDTO>> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusDTO dto)
        {
            var (success, error) = await _orderService.UpdateStatusAsync(id, dto.Status);
            if (!success) return BadRequest(new { message = error });
            return NoContent();
        }
    }
}

