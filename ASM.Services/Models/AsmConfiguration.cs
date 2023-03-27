namespace ASM.Services.Models
{
    public class AsmConfiguration
    {
        public string? AzureWebJobsStorage { get; set; }
        public string? SecretKey { get; set; }
        public long? AppId { get; set; }
        public string? RedirectUrl { get; set; }
        public Dictionary<string, string>? Countries { get; set; }
    }
}
