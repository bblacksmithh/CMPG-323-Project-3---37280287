using Models;

namespace EcoPower_Logistics.Repository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderDetailsAsync(int orderId);
    }
}
