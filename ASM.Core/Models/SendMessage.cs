namespace ASM.Core.Models
{
    public class SendMessage : RequestResponseBase
    {
        public long SellerId { get; set; }
        public long BuyerId { get; set; }
        public string Message { get; set; }
        public long PackId { get; set; }

    }
}
