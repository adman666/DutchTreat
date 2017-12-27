using System.Collections.Generic;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;

namespace DutchTreat.Data
{
    public interface IDutchRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<IEnumerable<Product>> GetAllByCategory(string category);
        Task<bool> SaveAll();
        Task<IEnumerable<Order>> GetAllOrders(bool includeItems);
        Task<Order> GetOrderById(string username, int id);
        Task AddEntity(object model);
        Task<IEnumerable<Order>> GetAllOrdersByUser(string username, bool includeItems);
        Task AddOrder(Order newOrder);
    }
}