using Confluent.Kafka;
using payment.service.Handlers;
using payment.service.Infrastructure;
using payment.service.Interfaces;
using System.Collections.Generic;

namespace payment.service.Services
{
    public class PaymentEventConsumerService : EventConsumerService
    {
        public PaymentEventConsumerService(ConsumerConfig config, string topic, string consumerGroup) : base(config, topic, consumerGroup)
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
            handlers.Add("FlightRequestSucceedEvent", new FlightRequestSucceedHandler());
            //handlers.Add("PaymentRequestFailedEvent", new PaymentRequestFailedHandler());

            return handlers;
        }
    }
}
