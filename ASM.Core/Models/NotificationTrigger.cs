﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Core.Models
{
    public class NotificationTrigger
    {
        public string topic { get; set; }
        public string resource { get; set; }
        public int user_id { get; set; }
        public long application_id { get; set; }
        public DateTime? sent { get; set; }
        public int attempts { get; set; }
        public DateTime? received { get; set; }

        public long OrderId 
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
    }
}
