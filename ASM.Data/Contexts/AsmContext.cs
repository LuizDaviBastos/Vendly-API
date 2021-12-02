using ASM.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Data.Contexts
{
    public class AsmContext : DbContext
    {
        public AsmContext(DbContextOptions options): base(options) { }

        public DbSet<Seller> Sellers { get; set; }
    }
}
