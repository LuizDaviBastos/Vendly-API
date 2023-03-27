namespace ASM.Services.Models
{
    public class SendMessage : RequestResponseBase
    {
        public long MeliSellerId { get; set; }
        public long BuyerId { get; set; }
        public string Message { get; set; }
        public long PackId { get; set; }

    }
}
