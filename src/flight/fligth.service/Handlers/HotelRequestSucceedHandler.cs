using fligth.service.Infrastructure;
using fligth.service.Interfaces;
using Newtonsoft.Json;

namespace fligth.service.Handlers
{
    public class HotelRequestSucceedHandler : IDomainEventHandler
    {
        public HotelRequestSucceedHandler()
        {

        }

        public void Handler(string message)
        {
            var orderRequestEventReceived = JsonConvert.DeserializeObject<OrderRequestEvent>(message);

            // Validate ...
            // Processing ...
            // Saving Database ...

            //if success
            if (orderRequestEventReceived.FlightId > 0)
                ProducerService.Producer("FlightRequestSucceedEvent", orderRequestEventReceived);
            //if failed
            else
                ProducerService.Producer("FlightRequestFailedEvent", orderRequestEventReceived);
        }

    }
}
