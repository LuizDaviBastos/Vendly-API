using ASM.Data.Documents;
using ASM.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASM.Data.Entities
{
    public class PaymentInformation : DocumentBase
    {
        public StatusEnum? Status { get; set; }
        public DateTime? ExpireIn { get; set; }
        public Seller Seller { get; set; }
    }
}
