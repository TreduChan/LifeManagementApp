using LifeManagement.Data;
using LifeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LifeManagement.Services
{
    // The only class that talks to the database.
    public class TodoRepository : ITodoRepository
    {
        // A factory instead of one shared DbContext: a DbContext is meant to be
        // short-lived, so every method creates a fresh one and disposes it at the end.
        private readonly IDbContextFactory<AppDbContext> _dbFactory;

        // DI passes the factory in through the constructor (constructor injection)
        public TodoRepository(IDbContextFactory<AppDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        // async: reading the file must not block the UI thread, or the app would freeze
        public async Task<List<TodoItem>> GetAllAsync()
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            return await db.Todos.ToListAsync();
        }

        public async Task AddAsync(TodoItem item)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            db.Todos.Add(item);
            // Nothing is written to the file until SaveChangesAsync (an INSERT here)
            await db.SaveChangesAsync();
        }

        public async Task UpdateAsync(TodoItem item)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            db.Todos.Update(item);   // finds the row by Id and becomes an UPDATE
            await db.SaveChangesAsync();
        }
    }
}
