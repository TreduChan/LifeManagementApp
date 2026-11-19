using LifeManagement.Data;
using LifeManagement.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LifeManagement
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // ---------- database ----------
            // AppDataDirectory is a folder the app is always allowed to write to, on every platform
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "lifemanagement.db3");
            builder.Services.AddDbContextFactory<AppDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            // Singleton = one shared instance for the whole app.
            // The repository holds no data of its own, so sharing it is safe.
            builder.Services.AddSingleton<ITodoRepository, TodoRepository>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            // Create the database file and the Todos table on first start.
            // EnsureCreated does nothing if the database already exists.
            using (var db = app.Services.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext())
            {
                db.Database.EnsureCreated();
            }

            return app;
        }
    }
}
