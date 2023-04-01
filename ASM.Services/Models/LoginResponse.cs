using ASM.Data.Entities;

namespace ASM.Services.Models
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public bool HasMeliAccount { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public Seller? Data { get; set; }
    }
}
