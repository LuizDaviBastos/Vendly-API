namespace ASM.DTO
{
    public class SellerDTO
    {
        public long SellerId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string? Message { get; set; }
        public PaymentInformationDTO BillingInformation { get; set; }
    }
}
