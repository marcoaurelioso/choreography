namespace order.service.Models
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using MongoDB.Bson;
    using System.Linq;

    public class OrderRepository : IOrderRepository
    {
        private readonly IOrderContext _context;

        public OrderRepository(IOrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderModel>> GetAllOrders()
        {
            return await _context
                            .Orders
                            .Find(_ => true)
                            .ToListAsync();
        }

        public Task<OrderModel> GetOrder(long id)
        {
            FilterDefinition<OrderModel> filter = Builders<OrderModel>.Filter.Eq(m => m.Id, id);
            return _context
                    .Orders
                    .Find(filter)
                    .FirstOrDefaultAsync();
        }

        public async Task Create(OrderModel orderModel)
        {
            await _context.Orders.InsertOneAsync(orderModel);
        }

        public async Task<bool> Update(OrderModel orderModel)
        {
            ReplaceOneResult updateResult =
                await _context
                        .Orders
                        .ReplaceOneAsync(
                            filter: g => g.Id == orderModel.Id,
                            replacement: orderModel);
            return updateResult.IsAcknowledged
                && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(long id)
        {
            FilterDefinition<OrderModel> filter = Builders<OrderModel>.Filter.Eq(m => m.Id, id);
            DeleteResult deleteResult = await _context
                                              .Orders
                                              .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<long> GetNextId()
        {
            return await _context.Orders.CountDocumentsAsync(new BsonDocument()) + 1;
        }
    }


}