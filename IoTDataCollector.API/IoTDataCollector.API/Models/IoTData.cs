namespace IoTDataCollector.API.Models
{
    public class IoTData
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public int Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
