namespace ASM.Services.Models
{
    public class SendMessageRequest
    {
        public SendMessageRequest() { }
        public SendMessageRequest(long from, long to, string text, IList<string>? attachments = null) 
        {
            this.from = new FromTo { user_id = from };
            this.to = new FromTo { user_id = to };
            this.text = text;
            this.attachments = attachments;
        }

        public FromTo from { get; set; }
        public FromTo to { get; set; }
        public string text { get; set; }
        public IList<string>? attachments { get; set; } = null;

        public class FromTo
        {
            public long user_id { get; set; }
        }
    }
}
