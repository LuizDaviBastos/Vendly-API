using ASM.Data.Entities;
using ASM.Services.Models;

namespace ASM.Services.Response
{
    public class SellerInfoResponse
    {
        public Seller? Seller { get; set; }
        public SellerInfo? MeliInfo { get; set; }
    }
}
