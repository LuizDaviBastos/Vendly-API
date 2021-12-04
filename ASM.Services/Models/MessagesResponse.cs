using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Services.Models
{
    public class MessagesResponse
    {
        public object paging { get; set; }
        public object conversation_status { get; set; }
        public List<Message> messages { get; set; }
        public int seller_max_message_length { get; set; }
        public int buyer_max_message_length { get; set; }

        public class From
        {
            public int user_id { get; set; }
        }

        public class To
        {
            public int user_id { get; set; }
        }

        public class MessageDate
        {
            public DateTime received { get; set; }
            public DateTime available { get; set; }
            public DateTime notified { get; set; }
            public DateTime created { get; set; }
            public object read { get; set; }
        }

        public class MessageModeration
        {
            public string status { get; set; }
            public object reason { get; set; }
            public string source { get; set; }
            public DateTime moderation_date { get; set; }
        }

        public class MessageResource
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Message
        {
            public string id { get; set; }
            public string site_id { get; set; }
            public long client_id { get; set; }
            public From from { get; set; }
            public To to { get; set; }
            public string status { get; set; }
            public object subject { get; set; }
            public string text { get; set; }
            public MessageDate message_date { get; set; }
            public MessageModeration message_moderation { get; set; }
            public object message_attachments { get; set; }
            public List<MessageResource> message_resources { get; set; }
            public bool conversation_first_message { get; set; }
        }

    }
}
