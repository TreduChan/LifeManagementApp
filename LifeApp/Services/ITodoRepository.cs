using LifeManagement.Models;

namespace LifeManagement.Services
{
    // ViewModels depend on this interface, not on EF Core directly.
    // That keeps database code out of the ViewModels and lets tests use a fake.
    public interface ITodoRepository
    {
        Task<List<TodoItem>> GetAllAsync();
        Task AddAsync(TodoItem item);
        Task UpdateAsync(TodoItem item);
        Task<TodoItem?> GetByIdAsync(int id);   // null if no todo has that Id
        Task DeleteAsync(TodoItem item);
    }
}
