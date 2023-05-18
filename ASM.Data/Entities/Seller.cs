using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ASM.Data.Entities
{
    public class Seller : IdentityUser<Guid>, IEntityBase
    {
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public string? ConfirmationCode { get; set; }

        public virtual PaymentInformation BillingInformation { get; set; }
        public virtual IList<MeliAccount> MeliAccounts { get; set; }   
    }
}
