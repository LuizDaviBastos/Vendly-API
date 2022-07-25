using ASM.Data;
using ASM.Data.Interfaces;
using ASM.Services.Interfaces;
using ASM.Services.Models;
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

            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://luiz:80849903D@asmserveless.m1ukj.mongodb.net/?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase("ASMDatabase");

            services.AddScoped<IMongoDatabase>(x => database);
            //services.AddDbContext<AsmContext>(x => x.UseSqlServer(configuration[((configuration?["Invironment"] ?? "") == "TEST" ? "ConnectionStringLocal" : "ConnectionString")]));

            return services;
        }
    }
}
