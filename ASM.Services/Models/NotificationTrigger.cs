namespace ASM.Services.Models
{
    public class NotificationTrigger
    {
        public string? topic { get; set; }
        public string? resource { get; set; }
        public int user_id { get; set; }
        public long application_id { get; set; }
        public DateTime? sent { get; set; }
        public int attempts { get; set; }
        public DateTime? received { get; set; }

        public long TopicId 
        { 
            get
            {
                if (!string.IsNullOrEmpty(resource) && resource.Contains("/") && resource.Split("/").Length >= 3)
                {
                    if(long.TryParse(resource.Split("/")[2], out long orderId))
                    {
                        return orderId;
                    }
                }
                return 0;
            } 
        }

        public bool IsOrderV2 { get => topic == "orders_v2"; }
        public bool IsFeedback { get => topic == "orders_feedback"; }
        public bool OrderIdIsValid { get => this.TopicId != 0; }
    }
}
