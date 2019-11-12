using Newtonsoft.Json;
using order.service.Infrastructure;
using order.service.Interfaces;

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

            // Compensation
            ProducerService.Producer("OrderRequestFailedEvent", orderRequestEventReceived);
        }

    }
}
