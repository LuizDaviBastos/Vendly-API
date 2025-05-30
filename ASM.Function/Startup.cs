﻿using ASM.Function.Services;
using ASM.Services.Helpers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(ASM.Function.Startup))]
namespace ASM.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAsmServices(builder.GetContext().Configuration);
            builder.Services.AddScoped<SendMessageService>();
        }
    }
}
