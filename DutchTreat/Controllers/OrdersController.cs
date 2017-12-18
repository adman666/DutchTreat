using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository _dutchRepository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;

        public OrdersController(IDutchRepository dutchRepository, ILogger<OrdersController> logger, IMapper mapper)
        {
            _dutchRepository = dutchRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders(bool includeItems = true)
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(
                    await _dutchRepository.GetAllOrders(includeItems)));
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
                    return Ok(_mapper.Map<Order, OrderViewModel>(order));

                return NotFound();
            }
            catch (Exception exception)
            {
                _logger.LogError($"Failed to get order for id {id}: {exception}");
                return BadRequest("Failed to get order");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = _mapper.Map<OrderViewModel, Order>(model);

                    if (newOrder.OrderDate == DateTime.MinValue)
                        newOrder.OrderDate = DateTime.Now;

                    await _dutchRepository.AddEntity(newOrder);
                    if (!await _dutchRepository.SaveAll())
                        return BadRequest(ModelState);

                    return Created($"/api/orders/{newOrder.Id}", _mapper.Map<Order, OrderViewModel>(newOrder));
                }

                return BadRequest(ModelState);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Failed to save a new order: {exception}");
            }

            return BadRequest("Failed to save a new order");
        }
    }
}