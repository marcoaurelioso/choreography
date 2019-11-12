using fligth.service.Infrastructure;
using fligth.service.Interfaces;
using Newtonsoft.Json;

namespace fligth.service.Handlers
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
            ProducerService.Producer("FlightRequestFailedEvent", orderRequestEventReceived);
        }

    }
}
