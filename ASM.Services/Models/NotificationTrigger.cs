namespace ASM.Services.Models
{
    public class NotificationTrigger
    {
        public string? topic { get; set; }
        public string? resource { get; set; }
        public int user_id { get; set; }
        public long? application_id { get; set; }
        public DateTime? sent { get; set; }
        public int? attempts { get; set; }
        public DateTime? received { get; set; }
        public string? trackingNumber{ get; set; }
        public string? trackingUrl { get; set; }

        public long TopicId 
        { 
            get
            {
                if (!string.IsNullOrEmpty(resource) && resource.Contains("/") && resource.Split("/").Length >= 3)
                {
                    if(long.TryParse(resource.Split("/")[2], out long topicId))
                    {
                        return topicId;
                    }
                }
                return 0;
            } 
        }

        public bool IsOrderV2 { get => topic == "orders_v2"; }
        public bool IsFeedback { get => topic == "orders_feedback"; }
        public bool TopicIdIsValid { get => this.TopicId != 0; }
        public bool IsShipping { get => topic == "shipments"; }
        public long OrderId { get; set; }

    }

    public class NotificationTriggerPayments
    {

        public string? action { get; set; }
        //public long application_id { get; set; }
        public MePaWebhookData? data { get; set; }
        //public string? entity { get; set; }
        //public string? id { get; set; }
        public string? type { get; set; }
        //public string? live_mode { get; set; }
        //public string? api_version { get; set; }
    }

    public class MePaWebhookData
    {
        public string id { get; set; }
    }

    /*
        "action": "payment.created",
   "api_version": "v1",
   "data": {
       "id": "1315305115"
   },
   "date_created": "2023-05-28T00:44:19Z",
   "id": 106067089602,
   "live_mode": false,
   "type": "payment",
   "user_id": "1384573578"

    */


    /*
     * criado assinaturea
     {
    "action": "created",
    "application_id": 6300258978976800,
    "data": {
        "id": "8aef01a4883161b001885f06d9391a79"
    },
    "date": "2023-05-27T21:04:24Z",
    "entity": "preapproval",
    "id": 106059658567,
    "type": "subscription_preapproval",
    "version": 0
}
     
     */


    /*
     {
    "action": "updated",
    "application_id": 6300258978976800,
    "data": {
        "id": "8aef01a4883161b001885f06d9391a79"
    },
    "date": "2023-05-27T21:12:22Z",
    "entity": "preapproval",
    "id": 106059757011,
    "type": "subscription_preapproval",
    "version": 2
}
     
     */



}
