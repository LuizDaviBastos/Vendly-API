namespace ASM.Api.Models
{
    public class UpdateMessage
    {
        public long SellerId { get; set; }
        public string Message { get; set; }
        public bool? AfterSellerMessageEnabled { get; set; }
    }
}
