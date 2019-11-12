namespace order.api.Models
{
    using MongoDB.Driver;

    public interface IOrderContext
    {
        IMongoCollection<OrderModel> Orders { get; }
    }
}