using Microsoft.EntityFrameworkCore;

namespace OrderService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            if (isProd)
            {
                using var serviceScope = app.ApplicationServices.CreateScope();
                var appDbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();

                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    appDbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }
        }
    }
}