namespace order.webapi.Models
{
    using order.webapi;
    using order.webapi.Config;
    using System;
    using MongoDB.Driver;

    public class OrderContext : IOrderContext
    {
        private readonly IMongoDatabase _db;

        public OrderContext(MongoDBConfig config)
        {
            var client = new MongoClient(config.ConnectionString);
            _db = client.GetDatabase(config.Database);
        }

        public IMongoCollection<OrderModel> Orders => _db.GetCollection<OrderModel>("Orders");
    }
}