using ASM.Data.Entities;

namespace ASM.Services.Models
{
    public class SendMessage : RequestResponseBase
    {
        public long MeliSellerId { get; set; }
        public long BuyerId { get; set; }
        public string Message { get; set; }
        public string PackId { get; set; }
        public IList<string>? Attachments { get; set; }
    }
}
