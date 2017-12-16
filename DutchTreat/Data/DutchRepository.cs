using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _context;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DutchContext context, ILogger<DutchRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            try
            {
                return await _context.Products
                    .OrderBy(x => x.Title)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Falied to get all products: {ex}");
                return null;
            }
           
        }

        public async Task<IEnumerable<Product>> GetAllByCategory(string category)
        {
            return await _context.Products
                .Where(x => x.Category == category)
                .ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
