using ASM.Core.Models;
using ASM.Core.Repositories;
using ASM.Core.Services;
using ASM.Data.Common;
using ASM.Data.Contexts;
using ASM.Data.Entities;
using ASM.Imp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<IMeliService, MeliService>();
builder.Services.AddScoped<IRepository<Seller>, SellerRepository>();

builder.Services.AddDbContext<AsmContext>(x => x.UseSqlite("Data source =./AsmDb.db"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
