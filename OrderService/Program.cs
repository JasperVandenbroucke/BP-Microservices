using Microsoft.EntityFrameworkCore;
using OrderService.AsyncDataServices;
using OrderService.Data;
using OrderService.Data.Repository;
using OrderService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using a SQL database");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("OrdersSQLConnection"))
    );
}
else
{
    Console.WriteLine("--> Using an InMemory database");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("OrderInMemoryDb")
    );
}

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddHttpClient<IShoppingCartDataClient, ShoppingCartDataClient>();

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

PrepDb.PrepPopulation(app, app.Environment.IsProduction());

var messageBus = app.Services.GetRequiredService<IMessageBusClient>();
await messageBus.InitializeAsync();

app.Run();
