using ASM.Data;
using ASM.Data.Interfaces;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace ASM.Services.Helpers
{
    public static class Extensions
    {
        public static IServiceCollection AddAsmServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<IMeliService, MeliService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton(x => configuration.Get<AsmConfiguration>());

            string connectionString = configuration.GetConnectionString("AsmConnection");
            services.AddDbContext<AsmContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}
