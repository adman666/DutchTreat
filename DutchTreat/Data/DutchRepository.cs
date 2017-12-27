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

        public async Task<IEnumerable<Order>> GetAllOrders(bool includeItems)
        {
            if (includeItems)
                return await _context.Orders
                    .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                    .ToListAsync();

            return await _context.Orders
                .ToListAsync();
        }

        public async Task<Order> GetOrderById(string username, int id)
        {
            return await _context.Orders
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id && x.User.UserName == username);
        }

        public async Task AddEntity(object model)
        {
            await _context.AddAsync(model);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersByUser(string username, bool includeItems)
        {
            if (includeItems)
                return await _context.Orders
                    .Where(x => x.User.UserName == username)
                    .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                    .ToListAsync();

            return await _context.Orders
                .ToListAsync();
        }

        public async Task AddOrder(Order newOrder)
        {
            foreach (var item in newOrder.Items)
            {
                item.Product = _context.Products.Find(item.Product.Id);
            }

            await AddEntity(newOrder);
        }
    }
}