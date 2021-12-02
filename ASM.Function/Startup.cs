using ASM.Core.Repositories;
using ASM.Data.Common;
using ASM.Data.Entities;
using ASM.Imp.Helpers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(ASM.Function.Startup))]
namespace ASM.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAsmServices(builder.GetContext().Configuration);
        }
    }
}
