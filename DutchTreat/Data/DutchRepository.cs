using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _context;

        public DutchRepository(DutchContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products
                .OrderBy(x => x.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllByCategory(string category)
        {
            return await _context.Products
                .Where(x => x.Category == category)
                .ToListAsync();
        }
    }
}
