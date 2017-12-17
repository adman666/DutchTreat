using System;
using System.Threading.Tasks;
using DutchTreat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    public class ProductsController : Controller
    {
        private readonly IDutchRepository _dutchRepository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IDutchRepository dutchRepository, ILogger<ProductsController> logger)
        {
            _dutchRepository = dutchRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _dutchRepository.GetAll());
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get products: {e}");
                return BadRequest("Failed to get products");
            }
        }
    }
}