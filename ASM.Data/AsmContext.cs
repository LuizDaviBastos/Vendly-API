using ASM.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASM.Data
{
    public class AsmContext : DbContext
    {
        public AsmContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<Seller> Sellers { get; set; }
        public virtual DbSet<SellerMessage> SellerMessages { get; set; }
        public virtual DbSet<PaymentInformation> PaymentInformations { get; set; }
        public virtual DbSet<MeliAccount> MeliAccounts { get; set; }
        
    }
}
