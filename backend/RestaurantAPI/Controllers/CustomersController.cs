using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.DTOs;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> Create([FromBody] CreateCustomerDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _customerService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetOrders), new { id = created.CustomerId }, created);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetAll()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        [Authorize]
        [HttpGet("{id:int}/orders")]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrders(int id)
        {
            var orders = await _customerService.GetOrdersAsync(id);
            return Ok(orders);
        }
    }
}

