namespace ASM.Services.Models
{
    public class ShipmentResponseByOrder : RequestResponseBase
    {
        public long id { get; set; }
        public string mode { get; set; }
        public string created_by { get; set; }
        public long order_id { get; set; }
        public double order_cost { get; set; }
        public double base_cost { get; set; }
        public string site_id { get; set; }
        public string status { get; set; }
        public object substatus { get; set; }
    }

}
