using ASM.Services.Helpers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(ASM.PreFunction.Startup))]
namespace ASM.PreFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAsmServices(builder.GetContext().Configuration);
        }
    }
}
