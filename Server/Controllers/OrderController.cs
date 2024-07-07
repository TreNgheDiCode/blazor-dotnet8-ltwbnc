using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Interfaces;

namespace Server.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [AllowAnonymous]
    public class OrderController(IOrderRepo orderRepo) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> GetOrders(int? page, int? pageSize)
        {
            var result = await orderRepo.GetOrders(page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(string id)
        {
            var result = await orderRepo.GetOrder(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(CreateOrderDTO order)
        {
            var result = await orderRepo.AddOrder(order);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> UpdateOrder(string id, UpdateOrderDTO order)
        {
            var result = await orderRepo.UpdateOrder(id, order);
            return Ok(result);
        }
    }
}
