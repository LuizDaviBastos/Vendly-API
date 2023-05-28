namespace ASM.Services.Models.Mepa
{
    public class PaymentResponse : RequestResponseBase
    {
        public long? id { get; set; }
        public DateTime? date_created { get; set; }
        public DateTime? date_approved { get; set; }
        public DateTime? date_last_updated { get; set; }
        public DateTime? money_release_date { get; set; }
        public string payment_method_id { get; set; }
        public string payment_type_id { get; set; }
        public string status { get; set; }
        public string status_detail { get; set; }
        public string currency_id { get; set; }
        public string description { get; set; }
        //public long? collector_id { get; set; }
        //public Payer payer { get; set; }
        public Metadata metadata { get; set; }
        //public AdditionalInfo additional_info { get; set; }
        public double? transaction_amount { get; set; }
        public double? transaction_amount_refunded { get; set; }
        public double? coupon_amount { get; set; }
        //public TransactionDetails transaction_details { get; set; }
        //public int? installments { get; set; }
        //public Card card { get; set; }
    }
    
    public class AdditionalInfo
    {
    }

    public class Card
    {
    }

    public class Identification
    {
        public string type { get; set; }
        public int number { get; set; }
    }

    public class Metadata
    {
        public Guid? sellerId { get; set; }
    }

    public class Payer
    {
        public long id { get; set; }
        public string email { get; set; }
        public Identification identification { get; set; }
        public string type { get; set; }
    }

    public class TransactionDetails
    {
        public int net_received_amount { get; set; }
        public int total_paid_amount { get; set; }
        public int overpaid_amount { get; set; }
        public int installment_amount { get; set; }
    }

}
