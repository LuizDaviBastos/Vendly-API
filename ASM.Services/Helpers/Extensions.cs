using ASM.Data;
using ASM.Data.Interfaces;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace ASM.Services.Helpers
{
    public static class Extensions
    {
        public static IServiceCollection AddAsmServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<IMeliService, MeliService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISellerService, SellerService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddSingleton(x => configuration.Get<AsmConfiguration>());

            var client = new MongoClient("mongodb+srv://luiz:80849903D@asmserveless.m1ukj.mongodb.net/?retryWrites=true&w=majority");
            services.AddSingleton(client.GetDatabase("ASMAPP"));

            string connectionString = configuration.GetConnectionString("AsmConnection");
            services.AddScoped<AsmContext>(x => new(connectionString));


            return services;
        }

        public static string HideEmail(this string input)
        {
            if(string.IsNullOrEmpty(input)) return input;
            string pattern = @"(?<=[\w]{1})[\w\-._\+%]*(?=[\w]{1}@)";
            return Regex.Replace(input, pattern, m => new string('*', m.Length));
        }
    }
}
