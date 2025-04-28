using Microsoft.EntityFrameworkCore;
using Yape.Infrastructure.Postgresql.Database;

namespace Yape.API.Transaction.Configurations
{
    public static class DatabaseConfiguration
    {
        public static WebApplicationBuilder AddDatabaseConfiguration(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            if (connectionString == null)
            {
                throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
            }

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            return builder;
        }

        public static WebApplication ConfigureDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (dbContext != null)
                {
                    dbContext.Database.Migrate();
                }
            }
            return app;
        }
    }
}
