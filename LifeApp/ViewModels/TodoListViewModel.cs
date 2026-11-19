using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeManagement.Models;
using LifeManagement.Services;
using LifeManagement.Views;

namespace LifeManagement.ViewModels
{
    public partial class TodoListViewModel : ObservableObject
    {
        private readonly ITodoRepository _repository;

        // Everything loaded from the database. Todos below is the same items, sorted.
        private List<TodoItem> _allTodos = new();

        public TodoListViewModel(ITodoRepository repository)
        {
            _repository = repository;
        }

        // ObservableCollection: the CollectionView redraws itself when items are added or cleared
        public ObservableCollection<TodoItem> Todos { get; } = new();

        // The two sort options shown in the Picker
        public List<string> SortOptions { get; } = new() { "Priority", "Due date" };

        // Changing the Picker sets this property, and the toolkit then calls OnSelectedSortChanged
        [ObservableProperty]
        private string selectedSort = "Priority";

        partial void OnSelectedSortChanged(string value) => ApplySort();

        // Called by the page every time it appears, so a todo added on the add page shows up here
        public async Task LoadAsync()
        {
            _allTodos = await _repository.GetAllAsync();
            ApplySort();
        }

        private void ApplySort()
        {
            IEnumerable<TodoItem> sorted = SelectedSort == "Due date"
                ? _allTodos.OrderBy(t => t.DueDate).ThenByDescending(t => t.Priority)   // earliest first
                : _allTodos.OrderByDescending(t => t.Priority).ThenBy(t => t.DueDate);  // 5 first

            // Fill the existing collection instead of creating a new one,
            // so the binding keeps pointing at the same object
            Todos.Clear();
            foreach (var todo in sorted)
                Todos.Add(todo);
        }

        // Navigation by route name. The route is registered in AppShell.xaml.cs.
        [RelayCommand]
        private async Task AddTodoAsync()
        {
            await Shell.Current.GoToAsync(nameof(AddTodoPage));
        }
    }
}
