using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Data.Repository;
using ProductService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsProduction())
{
    // Use a SQL Server db for production
    Console.WriteLine("--> Using a SQL Server database");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ProductsSQLConnection"))
    );
}
else
{
    // Use an InMemory db for development
    Console.WriteLine("--> Using an InMemory database");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("ProductInMemoryDb")
    );
}

builder.Services.AddScoped<IProductRepo, ProductRepo>();

builder.Services.AddHttpClient<IShoppingCartDataClient, ShoppingCartDataClient>();

// Adding AutoMapper to the project
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

Console.WriteLine($"--> ShoppingCart dev endpoint: {builder.Configuration["ShoppingCartService"]}");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

PrepDb.PrepPopulation(app, app.Environment.IsProduction());

app.Run();
