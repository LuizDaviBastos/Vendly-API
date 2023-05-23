using ASM.Data;
using ASM.Data.Interfaces;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Google.Cloud.Firestore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace ASM.Services.Helpers
{
    public static class Extensions
    {
        public static IServiceCollection AddAsmServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISettingsService, SettingsService>();
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

            FirestoreDb firestormDb = new FirestoreDbBuilder
            {
                ProjectId = "asm-app-413c2",
                JsonCredentials = @"{
                ""type"": ""service_account"",
                ""project_id"": ""asm-app-413c2"",
                ""private_key_id"": ""a0baeaf409a00cd712f4aa0190a0764118a8d7a8"",
                ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQC4n+ytnvnMfuJn\nzwd1pVxPuNg4P2cXX9TwWJliE6w/ZjefBPy/GYxhFIwTcagS1QZftZag2Mc76U5u\nqPEGK6dJTFs3WBzbLgl8X5mrEVjkSTqXaOMiCXT2IVVSW3h0f+4PPYjV1TdbwgPs\n2Ib8bTrltRv32p4EdJRGQhmuhwFJXB2GmZ3bciR5osFDQ6S67WXRM4BS05E61EoA\nqYi9TiEyDhmQ0v8mYVb/59rgoWKHIFRsfuqN/I6pIId88qzh7FMggI3e/VEu/iUs\nlSUtkk7j4+nkZvnRcwQIqApc4QWlAmLKfaOyIkYGgBxJFC7utvpPad8w3mxoT7GJ\nZducd4vpAgMBAAECggEAGZ2u1AQbuqDcPvT9nvgbj8Ag1+UMI7UbMWHJnk8GdiFz\nlC+39bfQv2N/b+80F7DQ2pSyWozZT9m2FSqBjo6d/lCG0235SvvfOw90ncShZcM3\nSuy1nvJa9Q947B1e4CMj3591Dd4weR25N7JvQDTTmC2BvOjrRcj7Dah8MbfvmMLf\ntybMUKx0VOFDKd352UZTtTD8QG+GxF/6WRjjy2nIuv45reI45JShZHG7nBi/xJvg\n91J0zut0JkGlgivP8da0tVjNFzlpWwFvATKUlIyUGWmvfLU1IMrSg35pOki1KLIN\nKzIA6Kuedq9gMJ9m8+YR9mvleccVRSRXXon1VXZQVQKBgQDlfuWvOACB3afFVghs\nL2WQvVmFYuCi2AwlVE1ycVGpT/rSTenQQ65ZhUhJ2mI76FJ+2FJApXQklhO/jeXf\n7y5FFQpIQDrGtT1KEVpXuYJv3nMe+2XA9lEpFYbFDhjvMOgZPI8fQhP5bxUMsh1N\nJkGeVsqHtnYQRPnq6+ksqHkjIwKBgQDN8meCqAoWHSHOe7ZfVWLsbi03vLAIX9U2\n27xDOKgWkAmEsHWBsMvg7eFcGb85nScmNHDKo+pUksKTLeEwAIwmwxfVLJyEcGJY\nHk9jl6v21uF3QUZQwiIg0WBCDO6seOk4NHR4B/xOKNXvaQeBJtQSVfvFyvehwV8q\nK18gM/C7gwKBgAkmbxf2XqbO5KKMlJdjqGQF+KzFzXkQriNK4i+e7weWJcT+0ES6\nfhgZpVE0gNgsoiVmkJj9P805op3vlWvBSJH+jcNltDI6BbCPUo/O2LGHKAXjNiJk\nb8X4ksAGmN3okEh5TqeLZep7EFbKPzrVPKaIhVi14K0zYteIAmsCaJoLAoGASFen\nDplSebQgBPUl+dT3m8+T6KlKOJbZQZzsQ7yqJsrP3SFTFYxyAX/uErkkl+thLiVr\nnXL4xF6NJMAQAxmeIZuUSpiSHl+P3B5Bit1jVaDjsE1oksOu01JY+rqqOEF8wvaC\nFwvPD/F+PMvrC+4EvrAfcbo7REG12Q1FK/2yyVsCgYAJGcKB/HHZhtHXXjSyWSDX\n/+exZv+KDf1bC//dTjjp5ldH5Tlqv/AtICeUdkYfNWZSYgwboTLgF62P9QdxjrTF\n91TAujFL2mCeuhP/JKnsIpyc1pFkGcQohSaSBtOVt7sQoz/zxG92RuQ4SxJfXYjI\nYKCj5N2zO85G6AL+M5zbdg==\n-----END PRIVATE KEY-----\n"",
                ""client_email"": ""firebase-adminsdk-gb36p@asm-app-413c2.iam.gserviceaccount.com"",
                ""client_id"": ""114676005675215426870"",
                ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
                ""token_uri"": ""https://oauth2.googleapis.com/token"",
                ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
                ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-gb36p%40asm-app-413c2.iam.gserviceaccount.com"",
                ""universe_domain"": ""googleapis.com""
                }"
            }.Build();
            services.AddScoped<FirestoreDb>(x => firestormDb);

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
