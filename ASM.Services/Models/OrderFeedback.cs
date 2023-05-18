namespace ASM.Services.Models
{
    public class OrderFeedback : RequestResponseBase
    {
        public Sale sale { get; set; }
        public Purchase purchase { get; set; }
    }

    public class From
    {
        public string nickname { get; set; }
        public int? id { get; set; }
        public string status { get; set; }
        public int? points { get; set; }
    }

    public class FeedbackItem
    {
        public int? price { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string currency_id { get; set; }
    }

    public class Purchase
    {
        public object reason { get; set; }
        public FeedbackItem item { get; set; }
        public string role { get; set; }
        public List<object> extended_feedback { get; set; }
        public DateTime? date_created { get; set; }
        public bool? fulfilled { get; set; }
        public string rating { get; set; }
        public DateTime? visibility_date { get; set; }
        public object message { get; set; }
        public object has_seller_refunded_money { get; set; }
        public string site_id { get; set; }
        public bool? modified { get; set; }
        public From from { get; set; }
        public long? id { get; set; }
        public To to { get; set; }
        public object reply { get; set; }
        public long? order_id { get; set; }
        public string app_id { get; set; }
        public string status { get; set; }
    }

    public class Sale
    {
        public object reason { get; set; }
        public Item item { get; set; }
        public string role { get; set; }
        public List<object> extended_feedback { get; set; }
        public DateTime? date_created { get; set; }
        public bool? fulfilled { get; set; }
        public string rating { get; set; }
        public DateTime? visibility_date { get; set; }
        public object message { get; set; }
        public object has_seller_refunded_money { get; set; }
        public string site_id { get; set; }
        public bool? modified { get; set; }
        public From from { get; set; }
        public long? id { get; set; }
        public To to { get; set; }
        public object reply { get; set; }
        public long? order_id { get; set; }
        public string app_id { get; set; }
        public string status { get; set; }
    }

    public class To
    {
        public string nickname { get; set; }
        public int? id { get; set; }
        public string status { get; set; }
        public int? points { get; set; }
    }


}
