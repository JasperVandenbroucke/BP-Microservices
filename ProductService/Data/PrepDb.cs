using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
        }

        private static void SeedData(AppDbContext context)
        {
            Console.WriteLine("--> Attempting to migrate the database...");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not run migrations: {ex.Message}");
            }

            if (!context.Products.Any())
            {
                // If db is empty
                Console.WriteLine("--> Seeding data...");

                context.Products.AddRange(
                    new Product() { Name = "Hoodie", Price = 49.99 },
                    new Product() { Name = "Polo", Price = 24.99 },
                    new Product() { Name = "Blouse", Price = 29.99 },
                    new Product() { Name = "Jeans", Price = 59.99 },
                    new Product() { Name = "Short", Price = 24.99 },
                    new Product() { Name = "Rok", Price = 34.99 }
                );

                context.SaveChanges();
            }
            else
            {
                // If db is NOT empty
                Console.WriteLine("--> We already have data");
            }
        }
    }
}