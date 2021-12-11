namespace ASM.Services.Models
{
    public abstract class RequestResponseBase
    {
        public bool? Success { get; set; }
        public string? Message { get; set; }
    }
}
