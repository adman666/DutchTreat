using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/orders/{orderid}/items")]
    public class OrderItemsController : Controller
    {
        private readonly IDutchRepository _dutchRepository;
        private readonly ILogger<OrderItemsController> _logger;
        private readonly IMapper _mapper;

        public OrderItemsController(IDutchRepository dutchRepository, ILogger<OrderItemsController> logger, IMapper mapper)
        {
            _dutchRepository = dutchRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int orderId)
        {
            try
            {
                var order = await _dutchRepository.GetOrderById(User.Identity.Name, orderId);

                if (order != null)
                {
                    return Ok(_mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewMdoel>>(order.Items));
                }

            }
            catch (Exception exception)
            {
                _logger.LogError($"Failed to get order for id {orderId}: {exception}");
            }

            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int orderId, int id)
        {
            try
            {
                var order = await _dutchRepository.GetOrderById(User.Identity.Name, orderId);

                var item = order?.Items.FirstOrDefault(x => x.Id == id);
                if (item != null)
                {
                    return Ok(_mapper.Map<OrderItem, OrderItemViewMdoel>(item));
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"Failed to get order for id {orderId}: {exception}");
            }

            return NotFound();
        }
    }
}