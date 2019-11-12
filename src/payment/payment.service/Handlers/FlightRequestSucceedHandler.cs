using payment.service.Infrastructure;
using payment.service.Interfaces;
using Newtonsoft.Json;

namespace payment.service.Handlers
{
    public class FlightRequestSucceedHandler : IDomainEventHandler
    {
        public FlightRequestSucceedHandler()
        {

        }

        public void Handler(string message)
        {
            var orderRequestEventReceived = JsonConvert.DeserializeObject<OrderRequestEvent>(message);

            // Validate ...
            // Processing ...
            // Saving Database ...

            //if success
            if (orderRequestEventReceived.Value <= 100)
                ProducerService.Producer("PaymentRequestSucceedEvent", orderRequestEventReceived);
            //if failed
            else
                ProducerService.Producer("PaymentRequestFailedEvent", orderRequestEventReceived);

        }

    }
}
