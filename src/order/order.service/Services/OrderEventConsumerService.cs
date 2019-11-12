using Confluent.Kafka;
using order.service.Infrastructure;
using order.service.Handlers;
using order.service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace order.service.Services
{
    public class OrderEventConsumerService : EventConsumerService
    {
        public OrderEventConsumerService(ConsumerConfig config, string topic, string consumerGroup) : base(config, topic, consumerGroup)
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
            handlers.Add("HotelRequestFailedEvent", new HotelRequestFailedHandler());
            handlers.Add("PaymentRequestFailedEvent", new PaymentRequestFailedHandler());
            handlers.Add("PaymentRequestSucceedEvent", new PaymentRequestSucceedHandler());

            return handlers;
        }
    }
}
