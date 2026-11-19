using LifeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LifeManagement.Data
{
    // The DbContext is EF Core's "session" with the database:
    // it turns LINQ queries into SQL and tracks changes until SaveChangesAsync().
    public class AppDbContext : DbContext
    {
        // The options (which database, which file) come from MauiProgram,
        // so this class does not need to know where the file is stored.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Each DbSet becomes a table. This one becomes the "Todos" table.
        public DbSet<TodoItem> Todos => Set<TodoItem>();
    }
}
