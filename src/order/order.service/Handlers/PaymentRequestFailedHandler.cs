using Newtonsoft.Json;
using order.service.Config;
using order.service.Infrastructure;
using order.service.Interfaces;
using order.service.Models;

namespace order.service.Handlers
{
    public class PaymentRequestFailedHandler : IDomainEventHandler
    {
        public PaymentRequestFailedHandler()
        {

        }

        public void Handler(string message)
        {
            var orderRequestEventReceived = JsonConvert.DeserializeObject<OrderRequestEvent>(message);

            // Validate ...
            // Processing ...
            // Saving Database ...

            var orderContext = new OrderContext(Program.mongoDBconfig);
            var repo = new OrderRepository(orderContext);

            var orderItem = repo.GetOrder(orderRequestEventReceived.Id).Result;
            if (orderItem != null)
            {
                orderItem.Status = "Failed";
                var resultUpdate = repo.Update(orderItem);
            }

            // Compensation
            ProducerService.Producer("OrderRequestFailedEvent", orderRequestEventReceived);
        }

    }
}
