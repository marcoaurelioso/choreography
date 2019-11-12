using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using order.service.Services;
using System;
using System.IO;


namespace order.service
{
    public class Program
    {
        private static string consumer_bootstrapservers;
        private static string consumer_groupid;
        private static string consumer_subscribe;
        private static bool consumer_enableautocommit;
        //private static ArrayList myEventsAsConsumer = new ArrayList() { "HotelRequestFailedEvent", "PaymentRequestSucceedEvent", "PaymentRequestFailedEvent" };

        static void Main(string[] args)
        {
            Console.WriteLine("Order Service - Started");
            Console.WriteLine();

            var builder = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            consumer_bootstrapservers = configuration.GetSection("consumer:bootstrapservers").Value;
            consumer_groupid = configuration.GetSection("consumer:groupId").Value;
            consumer_subscribe = configuration.GetSection("consumer:subscribe").Value;
            consumer_enableautocommit = bool.Parse(configuration.GetSection("consumer:enableautocommit").Value);

            var conf = new ConsumerConfig
            {
                GroupId = consumer_groupid,
                BootstrapServers = consumer_bootstrapservers,
                EnableAutoCommit = consumer_enableautocommit,
                // Note: The AutoOffsetReset property determines the start offset in the event there are not yet any committed offsets for the 
                // consumer group for the topic/partitions of interest. By default, offsets are committed automatically, so in this example, 
                // consumption will only start from the earliest message in the topic 'my-topic' the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            //--do some message producer to test
            //OrderRequestEvent orderRequestEvent = new OrderRequestEvent();
            //orderRequestEvent.EventId = Guid.NewGuid();
            //orderRequestEvent.EventType = "OrderRequestCreatedEvent";
            //orderRequestEvent.Id = 1;
            //orderRequestEvent.HotelId = 1;
            //orderRequestEvent.HotelRoomId = 1;
            //orderRequestEvent.UserName = "Marco";
            //orderRequestEvent.Value = 120;
            //Producer(kafkaTopic, "OrderRequestCreatedEvent", orderRequestEvent);
            //orderRequestEvent.HotelId = 0;
            //Producer(kafkaTopic, "OrderRequestCreatedEvent", orderRequestEvent);
            //Producer(kafkaTopic, "FlightRequestFailedEvent", orderRequestEvent);

            //---

            OrderEventConsumerService consumerService = new OrderEventConsumerService(conf, consumer_subscribe, consumer_groupid);
            consumerService.CheckEvents();


       

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

    }
}
