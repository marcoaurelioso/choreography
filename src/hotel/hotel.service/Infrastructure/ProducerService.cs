using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;

namespace hotel.service.Infrastructure
{
    public static class ProducerService
    {
        private static string producer_bootstrapservers;
        private static string producer_topic;

        public static void Producer(string eventType, OrderRequestEvent orderRequestEvent)
        {
            var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            producer_bootstrapservers = configuration.GetSection("producer:bootstrapservers").Value;
            producer_topic = configuration.GetSection("producer:topic").Value;

            // Create the producer configuration
            //var producerConfig = new Dictionary<string, object> { { "bootstrap.servers", kafkaEndpoint } };
            var config = new ProducerConfig { BootstrapServers = producer_bootstrapservers };

            orderRequestEvent.EventType = eventType;
            var messageSerialized = JsonConvert.SerializeObject(orderRequestEvent);

            using (var p = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var dr = p.ProduceAsync(producer_topic, new Message<Null, string> { Value = messageSerialized });
                    //Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");

                    if (eventType.ToLower().Contains("succeed"))
                        Console.ForegroundColor = ConsoleColor.Green;
                    if (eventType.ToLower().Contains("failed"))
                        Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Producer: ' { eventType } ' - sent on Partition: { dr.Result.Partition.Value } with Offset: { dr.Result.Offset.Value }");
                    Console.ResetColor();
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                    Console.ResetColor();
                }
            }
        }

    }
}
