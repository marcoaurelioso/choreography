using System;

namespace hotel.service
{
    public class OrderRequestEvent
    {
        public Guid EventId { get; set; }
        public string EventType { get; set; }
        public long Id { get; set; }
        public int HotelId { get; set; }
        public int HotelRoomId { get; set; }
        public int FlightId { get; set; }
        public string UserName { get; set; }
        public decimal Value { get; set; }

        public OrderRequestEvent()
        { 
        }

        public OrderRequestEvent(string eventValue)
        {
            Array eventValueList = eventValue.Split(";");
            EventId = Guid.Parse(eventValueList.GetValue(0).ToString());
            EventType = eventValueList.GetValue(1).ToString();
            Id = long.Parse(eventValueList.GetValue(2).ToString());
            HotelId = int.Parse(eventValueList.GetValue(3).ToString());
            HotelRoomId = int.Parse(eventValueList.GetValue(4).ToString());
            FlightId = int.Parse(eventValueList.GetValue(5).ToString());
            UserName = eventValueList.GetValue(6).ToString();
            Value = decimal.Parse(eventValueList.GetValue(7).ToString());
        }
    }
}
