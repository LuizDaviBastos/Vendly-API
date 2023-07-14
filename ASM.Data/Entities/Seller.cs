using ASM.Data.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ASM.Data.Entities
{
    public class Seller : IdentityUser<Guid>, IEntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string? ConfirmationCode { get; set; }
        public bool? AcceptedTerms { get; set; }
        public bool? IsTestUser { get; set; }

        public virtual PaymentInformation BillingInformation { get; set; }
        public virtual IList<MeliAccount> MeliAccounts { get; set; }
        public virtual IList<SellerOrder> Orders { get; set; }
        public virtual IList<PaymentHistory> PaymentHistory { get; set; }
        public virtual IList<SellerFcmToken> SellerFcmTokens { get; set; }
    }
}
