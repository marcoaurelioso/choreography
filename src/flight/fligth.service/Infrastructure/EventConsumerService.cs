using Confluent.Kafka;
using fligth.service.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;

namespace fligth.service.Infrastructure
{
    public class EventConsumerService
    {
        private Dictionary<string, IDomainEventHandler> handlers;
        private ConsumerConfig consumerConfig;
        private string kafkaTopic;

        public EventConsumerService(ConsumerConfig config, string topic, string consumerGroup)
        {
            consumerConfig = config;
            kafkaTopic = topic;
            handlers = buildCommandHandlers();
        }

        public virtual Dictionary<string, IDomainEventHandler> buildCommandHandlers()
        {
            return new Dictionary<string, IDomainEventHandler>();
        }

        public void CheckEvents() {

            // Note: The AutoOffsetReset property determines the start offset in the event there are not yet any committed offsets for the 
            // consumer group for the topic/partitions of interest. By default, offsets are committed automatically, so in this example, 
            // consumption will only start from the earliest message in the topic 'my-topic' the first time you run the program.
            consumerConfig.AutoOffsetReset = AutoOffsetReset.Earliest;

            using (var c = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
            {
                c.Subscribe(kafkaTopic);

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) => {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);

                            //Business logic
                            if (cr.Value.ToLower().Contains("eventtype"))
                            {
                                var orderRequestEventReceived = JsonConvert.DeserializeObject<OrderRequestEvent>(cr.Value);

                                //if (myEventsAsConsumer.Contains(orderRequestEventReceived.EventType))
                                if (handlers.ContainsKey(orderRequestEventReceived.EventType))
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine();
                                    Console.WriteLine("Message Found:");
                                    Console.ResetColor();
                                    Console.WriteLine($"{ orderRequestEventReceived.EventType } - Topic: {cr.Topic} Partition: {cr.Partition} Offset: {cr.Offset} {cr.Value}");

                                    IDomainEventHandler handler = handlers.GetValueOrDefault(orderRequestEventReceived.EventType);
                                    handler.Handler(cr.Value);

                                    //if (orderRequestEventReceived.EventType == "OrderRequestCreatedEvent")
                                    //{
                                    //    // Validate ...
                                    //    // Processing ...
                                    //    // Saving Database ...

                                    //    //if success
                                    //    if (orderRequestEventReceived.HotelId > 0 && orderRequestEventReceived.HotelRoomId > 0)
                                    //    {
                                    //        //Producer(kafkaTopic, "HotelRequestSucceedEvent", orderRequestEventReceived);
                                    //    }
                                    //    //if failed
                                    //    else
                                    //    {
                                    //        //Producer(kafkaTopic, "HotelRequestFailedEvent", orderRequestEventReceived);
                                    //    }
                                    //}

                                    //if (orderRequestEventReceived.EventType == "FlightRequestFailedEvent")
                                    //{
                                    //    // Compensation
                                    //    //Producer(kafkaTopic, "HotelRequestFailedEvent", orderRequestEventReceived);
                                    //}
                                }
                            }

                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }

                        // use the following code just to make demonstration
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    c.Close();
                }
            }


        }


    }
}