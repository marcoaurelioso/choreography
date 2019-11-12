using hotel.service.Infrastructure;
using hotel.service.Interfaces;
using Newtonsoft.Json;

namespace hotel.service.Handlers
{
    public class FlightRequestFailedHandler: IDomainEventHandler
    {
        public FlightRequestFailedHandler()
        {

        }

        public void Handler(string message)
        {
            var orderRequestEventReceived = JsonConvert.DeserializeObject<OrderRequestEvent>(message);

            // Validate ...
            // Processing ...
            // Saving Database ...

            // Compensation
            ProducerService.Producer("HotelRequestFailedEvent", orderRequestEventReceived);
        }

    }
}
