using System;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository _dutchRepository;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IDutchRepository dutchRepository, ILogger<OrdersController> logger)
        {
            _dutchRepository = dutchRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                return Ok(await _dutchRepository.GetAllOrders());
            }
            catch (Exception exception)
            {
                _logger.LogError($"Failed to get orders: {exception}");
                return BadRequest("Failed to get orders");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var order = await _dutchRepository.GetOrderById(id);

                if (order != null)
                {
                    return Ok(order);
                }

                return NotFound();
            }
            catch (Exception exception)
            {
                _logger.LogError($"Failed to get orders: {exception}");
                return BadRequest("Failed to get orders");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Order model)
        {
            try
            {
                _dutchRepository.AddEntity(model);
                _dutchRepository.SaveAll();
            }
            catch (Exception exception)
            {
                _logger.LogError($"Failed to save a new order: {exception}");
            }
        }
    }
}