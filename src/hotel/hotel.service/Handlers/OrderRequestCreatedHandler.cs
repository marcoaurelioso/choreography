using Confluent.Kafka;
using hotel.service.Infrastructure;
using hotel.service.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace hotel.service.Handlers
{
    public class OrderRequestCreatedHandler : IDomainEventHandler
    {
        //private static readonly string kafkaEndpoint = "localhost:9092";
        //private static readonly string kafkaTopic = "orderrequests";

        public OrderRequestCreatedHandler()
        {

        }

        public void Handler(string message)
        {
            //throw new NotImplementedException();

            var orderRequestEventReceived = JsonConvert.DeserializeObject<OrderRequestEvent>(message);

            // Validate ...
            // Processing ...
            // Saving Database ...

            //if success
            if (orderRequestEventReceived.HotelId > 0 && orderRequestEventReceived.HotelRoomId > 0)
                ProducerService.Producer("HotelRequestSucceedEvent", orderRequestEventReceived);
            //if failed
            else
                ProducerService.Producer("HotelRequestFailedEvent", orderRequestEventReceived);

        }

        //public static void Producer(string topic, string eventType, OrderRequestEvent orderRequestEvent)
        //{
        //    // Create the producer configuration
        //    //var producerConfig = new Dictionary<string, object> { { "bootstrap.servers", kafkaEndpoint } };
        //    var config = new ProducerConfig { BootstrapServers = kafkaEndpoint };

        //    orderRequestEvent.EventType = eventType;
        //    var messageSerialized = JsonConvert.SerializeObject(orderRequestEvent);

        //    // Create the producer
        //    //using (var producer = new Producer<Null, string>(producerConfig, null, new StringSerializer(Encoding.UTF8)))
        //    //{
        //    //    orderRequestEvent.EventType = eventType;
        //    //    var messageSerialized = JsonConvert.SerializeObject(orderRequestEvent);
        //    //    var result = producer.ProduceAsync(kafkaTopic, null, messageSerialized).GetAwaiter().GetResult();
        //    //    if (eventType.ToLower().Contains("succeed"))
        //    //        Console.ForegroundColor = ConsoleColor.Green;
        //    //    if (eventType.ToLower().Contains("failed"))
        //    //        Console.ForegroundColor = ConsoleColor.Red;
        //    //    Console.WriteLine($"Producer: ' { eventType } ' - sent on Partition: {result.Partition} with Offset: {result.Offset}");
        //    //    Console.ResetColor();
        //    //}


        //    using (var p = new ProducerBuilder<Null, string>(config).Build())
        //    {
        //        try
        //        {
        //            var dr = p.ProduceAsync(kafkaTopic, new Message<Null, string> { Value = messageSerialized });
        //            //Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");

        //            if (eventType.ToLower().Contains("succeed"))
        //                Console.ForegroundColor = ConsoleColor.Green;
        //            if (eventType.ToLower().Contains("failed"))
        //                Console.ForegroundColor = ConsoleColor.Red;
        //            Console.WriteLine($"Producer: ' { eventType } ' - sent on Partition: { dr.Result.Partition.Value } with Offset: { dr.Result.Offset.Value }");
        //            Console.ResetColor();
        //        }
        //        catch (ProduceException<Null, string> e)
        //        {
        //            Console.ForegroundColor = ConsoleColor.Yellow;
        //            Console.WriteLine($"Delivery failed: {e.Error.Reason}");
        //            Console.ResetColor();
        //        }
        //    }
        //}

    }
}
