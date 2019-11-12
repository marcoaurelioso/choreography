using Confluent.Kafka;
using hotel.service.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace hotel.service
{
    class Program
    {
        private static string consumer_bootstrapservers;
        private static string consumer_groupid;
        private static string consumer_subscribe;
        private static bool consumer_enableautocommit;

        static void Main(string[] args)
        {
            Console.WriteLine("Hotel Service - Started");
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

            ////--do some message producer to test
            //OrderRequestEvent orderRequestEvent = new OrderRequestEvent();
            //orderRequestEvent.EventId = Guid.NewGuid();
            //orderRequestEvent.EventType = "OrderRequestCreatedEvent";
            //orderRequestEvent.Id = 1;
            //orderRequestEvent.HotelId = 1;
            //orderRequestEvent.HotelRoomId = 1;
            //orderRequestEvent.UserName = "Marco";
            //orderRequestEvent.Value = 100;
            //ProducerService.Producer("OrderRequestCreatedEvent", orderRequestEvent);

            HotelEventConsumerService consumerService = new HotelEventConsumerService(conf, consumer_subscribe, consumer_groupid);
            consumerService.CheckEvents();

            Console.WriteLine();
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
