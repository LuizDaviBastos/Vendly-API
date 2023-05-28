namespace ASM.Services.Models.Mepa
{
    public class PaymentLinkParams
    {
        public List<PaymentItem> Items { get; set; }
        public Metadata Metadata { get; set; }
    }


    public class PaymentItem
    {
        public string title { get; set; }
        public int quantity { get; set; }
        public string currency_id { get; set; }
        public decimal unit_price { get; set; }
    }
}
