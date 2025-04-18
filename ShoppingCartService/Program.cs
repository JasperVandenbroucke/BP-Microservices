using Microsoft.EntityFrameworkCore;
using ShoppingCartService.AsyncDataServices;
using ShoppingCartService.Data;
using ShoppingCartService.Data.Repository;
using ShoppingCartService.EventProcessing;
using ShoppingCartService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Use the correct database
if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using a SQL Server database");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ShoppingCartsSQLConnection"))
    );
}
else
{
    Console.WriteLine("--> Using an InMemory database");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("ShoppingCartInMemoryDb")
    );
}

builder.Services.AddScoped<IShoppingCartRepo, ShoppingCartRepo>();

builder.Services.AddHttpClient<IProductDataClient, ProductDataClient>();

builder.Services.AddSingleton<IEventProcessor, EventProcessor>();

builder.Services.AddSingleton<MessageBusSub>();

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

if (app.Environment.IsProduction())
{
    // If production => migrate db
    PrepDb.PrepPopulation(app);
}

var messageBusSub = app.Services.GetRequiredService<MessageBusSub>();
await messageBusSub.InitializeRabbitMQAsync();

app.Run();
