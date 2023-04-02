namespace ASM.Api.Models
{
    public class IsAuthenticatedResponse
    {
        public bool HasMeliAccount { get; set; }
        public bool TokenIsValid { get; set; }
    }
}
