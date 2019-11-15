namespace order.service.Models
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IOrderRepository
    {
        // api/[GET]
        Task<IEnumerable<OrderModel>> GetAllOrders();

        // api/1/[GET]
        Task<OrderModel> GetOrder(long id);

        // api/[POST]
        Task Create(OrderModel order);

        // api/[PUT]
        Task<bool> Update(OrderModel order);

        // api/1/[DELETE]
        Task<bool> Delete(long Id);

        Task<long> GetNextId();
    }


}