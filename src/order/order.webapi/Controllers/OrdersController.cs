using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using order.webapi.Models;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System.Text;
using Newtonsoft.Json;
using NSwag.Annotations;

namespace order.webapi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly string kafkaEndpoint = "localhost:9092";
        private readonly string kafkaTopic = "orderrequests";
        private readonly IOrderRepository _repo;

        public OrdersController(IOrderRepository repo)
        {
            _repo = repo;
        }

        // GET api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderModel>>> Get()
        {
            return new ObjectResult(await _repo.GetAllOrders());
        }

        // GET api/orders/1
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderModel>> Get(long id)
        {
            var orderModel = await _repo.GetOrder(id);
            if (orderModel == null)
                return new NotFoundResult();
            
            return new ObjectResult(orderModel);
        }

        /// <summary>
        /// Create a specific Order Item. POST api/orders
        /// </summary>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<OrderModel>> Post([FromBody] OrderModel orderModel)
        {
            orderModel.Id = await _repo.GetNextId();
            orderModel.Status = "Pending";
            await _repo.Create(orderModel);

            //var orderItem = _repo.GetOrder(orderModel.Id).Result;
            //orderItem.Status = "Processing";
            //var resultUpdate = _repo.Update(orderItem);


            //--Publish Event/Message
            OrderRequestEvent orderRequestEvent = new OrderRequestEvent();
            orderRequestEvent.EventId = Guid.NewGuid();
            orderRequestEvent.Id = orderModel.Id;
            orderRequestEvent.EventType = "OrderRequestCreatedEvent";
            orderRequestEvent.HotelId = orderModel.HotelId;
            orderRequestEvent.HotelRoomId = orderModel.HotelRoomId;
            orderRequestEvent.FlightId = orderModel.FlightId;
            orderRequestEvent.UserName = orderModel.UserName;
            orderRequestEvent.Value = orderModel.Value;
            Producer(kafkaTopic, "OrderRequestCreatedEvent", orderRequestEvent);

            return new OkObjectResult(orderModel);
        }

        /// <summary>
        /// Update a specific Order Item. PUT api/orders/1
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderModel>> Put(long id, [FromBody] OrderModel orderModel)
        {
            var orderFromDb = await _repo.GetOrder(id);
            if (orderFromDb == null)
                return new NotFoundResult();
            orderModel.Id = orderFromDb.Id;
            //orderModel.InternalId = orderFromDb.InternalId;
            await _repo.Update(orderModel);
            return new OkObjectResult(orderModel);
        }

        // DELETE api/orders/1
        /// <summary>
        /// Delete a specific Order Item. DELETE api/orders/1
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var post = await _repo.GetOrder(id);
            if (post == null)
                return new NotFoundResult();
            await _repo.Delete(id);
            return new OkResult();
        }

        [SwaggerIgnore]
        public void Producer(string topic, string eventType, OrderRequestEvent orderRequestEvent)
        {
            // Create the producer configuration
            //var producerConfig = new Dictionary<string, object> { { "bootstrap.servers", kafkaEndpoint } };

            //Serialize
            //string serializedOrder = JsonConvert.SerializeObject(value);
            //var producer = new ProducerWrapper(this.config, "orderrequests");
            //await producer.writeMessage(serializedOrder);

            var config = new ProducerConfig { BootstrapServers = kafkaEndpoint };
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                orderRequestEvent.EventType = eventType;
                var messageSerialized = JsonConvert.SerializeObject(orderRequestEvent);

                var result = producer.ProduceAsync(kafkaTopic, new Message<Null, string> { Value = messageSerialized });

                producer.Flush(TimeSpan.FromSeconds(1));

                if (eventType.ToLower().Contains("succeed"))
                    Console.ForegroundColor = ConsoleColor.Green;
                if (eventType.ToLower().Contains("failed"))
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Producer: ' { eventType } ' - sent on Partition: {result.Result.Partition} with Offset: {result.Result.Offset}");
                Console.ResetColor();
            }

            //// Create the producer
            //using (var producer = new Producer<Null, string>(producerConfig, null, new StringSerializer(Encoding.UTF8)))
            //{
            //    orderRequestEvent.EventType = eventType;
            //    var messageSerialized = JsonConvert.SerializeObject(orderRequestEvent);
            //    var result = producer.ProduceAsync(kafkaTopic, null, messageSerialized).GetAwaiter().GetResult();
            //    if (eventType.ToLower().Contains("succeed"))
            //        Console.ForegroundColor = ConsoleColor.Green;
            //    if (eventType.ToLower().Contains("failed"))
            //        Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine($"Producer: ' { eventType } ' - sent on Partition: {result.Partition} with Offset: {result.Offset}");
            //    Console.ResetColor();
            //}
        }
    }


}