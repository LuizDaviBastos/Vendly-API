using System.Collections.Generic;

namespace ASM.Data.Entities
{
    public class Seller : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        public virtual PaymentInformation BillingInformation { get; set; }
        public virtual IList<SellerMessage> Messages { get; set; }
        public virtual IList<MeliAccount> MeliAccounts { get; set; }
    }
}
