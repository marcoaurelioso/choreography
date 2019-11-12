using Confluent.Kafka;
using hotel.service.Handlers;
using hotel.service.Infrastructure;
using hotel.service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace hotel.service.Services
{
    public class HotelEventConsumerService : EventConsumerService
    {
        public HotelEventConsumerService(ConsumerConfig config, string topic, string consumerGroup) : base(config, topic, consumerGroup)
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
            handlers.Add("OrderRequestCreatedEvent", new OrderRequestCreatedHandler());
            handlers.Add("FlightRequestFailedEvent", new FlightRequestFailedHandler());

            return handlers;
        }
    }
}
