namespace ASM.Data.Entities
{
    public class Seller : Entity
    {
        public long SellerId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string? Message { get; set; }
    }
}
