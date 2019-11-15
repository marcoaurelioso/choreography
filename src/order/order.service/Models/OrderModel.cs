using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace order.service.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    public class OrderModel
    {
        //A document in MongoDB requires an ObjectId id.This is a unique identifier that is used internally in the database
        [JsonIgnore]
        [BsonId]
        //[BsonRepresentation(BsonType.ObjectId)] 
        public ObjectId InternalId { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public long Id { get; set; }
        public int HotelId { get; set; }
        public int HotelRoomId { get; set; }
        public int FlightId { get; set; }
        [JsonIgnore]
        public string UserName { get; set; }
        public string Status { get; set; }
        public decimal Value { get; set; }
    }
}