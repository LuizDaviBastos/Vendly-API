using ASM.DTO.Enums;

namespace ASM.DTO
{
    public class PaymentInformationDTO
    {
        public StatusEnum? Status { get; set; }
        public DateTime? ExpireIn { get; set; }
        public SellerDTO Seller { get; set; }
    }
}