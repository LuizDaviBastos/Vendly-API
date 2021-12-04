using ASM.Core.Repositories;
using ASM.Data.Common;
using ASM.Data.Contexts;
using ASM.Data.Entities;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASM.Services.Helpers
{
    public static class Extensions
    {
        public static IServiceCollection AddAsmServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<IMeliService, MeliService>();
            services.AddScoped<IRepository<Seller>, SellerRepository>();
            services.AddDbContext<AsmContext>(x => x.UseSqlite(configuration["ConnectionString"]));
            services.AddSingleton(x => configuration.Get<AsmConfiguration>());

            return services;
        }
    }
}
