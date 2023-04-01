using ASM.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ASM.Data
{
    public class AsmContext : DbContext
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=asmdb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public AsmContext()
        {

        }

        public AsmContext(DbContextOptions options) : base(options)
        {
            
        }

        

        public virtual DbSet<Seller> Sellers { get; set; }
        public virtual DbSet<SellerMessage> SellerMessages { get; set; }
        public virtual DbSet<PaymentInformation> PaymentInformations { get; set; }
        public virtual DbSet<MeliAccount> MeliAccounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
                base.OnConfiguring(optionsBuilder);
            }
        }
    }
}
