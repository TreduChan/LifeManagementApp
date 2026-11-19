using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeManagement.Models;
using LifeManagement.Services;

namespace LifeManagement.ViewModels
{
    public partial class AddTodoViewModel : ObservableObject
    {
        // The ViewModel only knows the interface. DI decides which class it gets.
        private readonly ITodoRepository _repository;

        // DI calls this constructor and passes in the registered ITodoRepository
        public AddTodoViewModel(ITodoRepository repository)
        {
            _repository = repository;
        }

        // ---------- categories: ObservableCollection tells the Picker when an item is added ----------
        public ObservableCollection<string> Categories { get; } = new() { "Study", "Home", "Health", "Work" };

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TitleCounter))]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string title = "";

        [ObservableProperty] private string notes = "";
        [ObservableProperty] private string category = "Study";
        [ObservableProperty] private DateTime dueDate = DateTime.Today.AddDays(1);
        [ObservableProperty] private double priority = 3;   // double because Stepper.Value is a double
        [ObservableProperty] private bool reminder = true;
        [ObservableProperty] private string result = "";

        public string TitleCounter => $"{Title?.Length ?? 0}/40";

        // This is where an empty title is blocked: while it returns false the Save button is disabled
        // and Enter in the title field does nothing.
        private bool CanSave() => !string.IsNullOrWhiteSpace(Title) && Title.Length <= 40;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddCategoryCommand))]
        private string newCategory = "";

        private bool CanAddCategory()
        {
            string name = NewCategory?.Trim() ?? "";
            return name.Length > 0 && !Categories.Any(c => string.Equals(c, name, StringComparison.OrdinalIgnoreCase));
        }

        [RelayCommand(CanExecute = nameof(CanAddCategory))]
        private void AddCategory()
        {
            string name = NewCategory.Trim();
            Categories.Add(name);
            Category = name;
            NewCategory = "";
        }

        // async Task: the toolkit still generates "SaveCommand" (the Async suffix is dropped)
        // and the UI stays responsive while the database is written.
        [RelayCommand(CanExecute = nameof(CanSave))]
        private async Task SaveAsync()
        {
            // Copy the form fields into a model object – this is what goes to the database
            var item = new TodoItem
            {
                Title = Title.Trim(),
                Notes = Notes.Trim(),
                Category = Category,
                Priority = (int)Priority,
                DueDate = DueDate,
                Reminder = Reminder,
                IsDone = false
            };

            await _repository.AddAsync(item);

            // SQLite filled in the Id during SaveChangesAsync, so it proves the row exists
            Result = $"Saved #{item.Id}: {item.Title}";

            // Empty the form for the next todo (this also disables Save again)
            Title = "";
            Notes = "";
            Priority = 3;
            DueDate = DateTime.Today.AddDays(1);
        }
    }
}
