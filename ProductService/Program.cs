using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Use the correct SQL Server db
if (builder.Environment.IsProduction())
{
    //TODO: use db deployed in kubernetes
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ProductsConnDev"))
    );
}

builder.Services.AddScoped<IProductRepo, ProductRepo>();

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

PrepDb.PrepPopulation(app);

app.Run();
