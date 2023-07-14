using ASM.Data;
using ASM.Data.Entities;
using ASM.Services.Helpers;
using ASM.Services.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly AsmConfiguration asmConfiguration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            asmConfiguration = configuration.Get<AsmConfiguration>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddAsmServices(Configuration);
            services.AddIdentity<Seller, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireNonAlphanumeric = true;
                /*
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;*/

                // Adicione outras configurações de senha aqui, se necessário
            }).AddEntityFrameworkStores<AsmContext>()
            .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(24);
            });


            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(asmConfiguration.JwtKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            /* services.AddAuthorization(options =>
             {
                 options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
             });*/

            /*services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = long.MaxValue;
                options.MultipartBodyLengthLimit = long.MaxValue; // if don't set default value is: 128 MB
                options.MultipartHeadersLengthLimit = long.MaxValue;
            });*/

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AsmContext.Migrate(Configuration.GetConnectionString("AsmConnection"));
            if (env.IsDevelopment())
            {
                app.UseSwaggerUI();
                app.UseSwagger();

                app.UseDeveloperExceptionPage();
            }
            else
            {

                app.UseExceptionHandler("/Error");

                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
