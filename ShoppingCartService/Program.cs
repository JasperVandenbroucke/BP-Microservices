using Microsoft.EntityFrameworkCore;
using ShoppingCartService.Data;
using ShoppingCartService.Data.Repository;
using ShoppingCartService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Use an InMemory db for development
Console.WriteLine("--> Using an InMemory database");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ShoppingCartInMemoryDb")
);

builder.Services.AddScoped<IShoppingCartRepo, ShoppingCartRepo>();

builder.Services.AddHttpClient<IProductDataClient, ProductDataClient>();

// Adding AutoMapper to the project
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
