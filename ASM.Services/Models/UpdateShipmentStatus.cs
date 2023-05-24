namespace ASM.Services.Models
{
    public class UpdateShipmentStatus
    {
        public PayloadShipment payload { get; set; }
        public string tracking_number { get; set; }
        public string tracking_url { get; set; }
        public string status { get; set; }
        public string substatus { get; set; }
    }

    public class PayloadShipment
    {
        public int service_id { get; set; }
        public string comment { get; set; }
        public DateTime date { get; set; }
    }
}
