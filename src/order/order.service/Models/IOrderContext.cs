namespace order.service.Models
{
    using MongoDB.Driver;

    public interface IOrderContext
    {
        IMongoCollection<OrderModel> Orders { get; }
    }
}