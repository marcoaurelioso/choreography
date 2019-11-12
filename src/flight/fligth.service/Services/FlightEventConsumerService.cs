using Confluent.Kafka;
using fligth.service.Handlers;
using fligth.service.Infrastructure;
using fligth.service.Interfaces;
using System.Collections.Generic;

namespace fligth.service.Services
{
    public class FlightEventConsumerService : EventConsumerService
    {
        public FlightEventConsumerService(ConsumerConfig config, string topic, string consumerGroup) : base(config, topic, consumerGroup)
        {

        }

        //public void CheckEvents()
        //{
        //    CheckEvents();
        //}

        //public override Dictionary<string, IDomainEventHandler> buildCommandHandlers()
        public override Dictionary<string, IDomainEventHandler> buildCommandHandlers()
        {
            Dictionary<string, IDomainEventHandler> handlers = new Dictionary<string, IDomainEventHandler>();
            handlers.Add("HotelRequestSucceedEvent", new HotelRequestSucceedHandler());
            handlers.Add("PaymentRequestFailedEvent", new PaymentRequestFailedHandler());

            return handlers;
        }
    }
}
