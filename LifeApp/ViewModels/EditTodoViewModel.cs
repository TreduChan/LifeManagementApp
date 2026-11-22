using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeManagement.Models;
using LifeManagement.Services;

namespace LifeManagement.ViewModels
{
    // IQueryAttributable: Shell calls ApplyQueryAttributes with the parameters from the route,
    // e.g. GoToAsync("EditTodoPage?id=5") gives query["id"] = "5"
    public partial class EditTodoViewModel : ObservableObject, IQueryAttributable
    {
        private readonly ITodoRepository _repository;

        // The todo loaded from the database. Save copies the form fields back into it.
        private TodoItem? _todo;

        // The parameter that came with the navigation
        private int _todoId;

        public EditTodoViewModel(ITodoRepository repository)
        {
            _repository = repository;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string title = "";

        [ObservableProperty] private string notes = "";
        [ObservableProperty] private DateTime dueDate = DateTime.Today;
        [ObservableProperty] private double priority = 3;
        [ObservableProperty] private bool isDone;

        // Runs before the page appears. We only store the id here; the page's OnAppearing loads the todo.
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("id", out var value) && int.TryParse(value?.ToString(), out int id))
                _todoId = id;
        }

        public async Task LoadAsync()
        {
            _todo = await _repository.GetByIdAsync(_todoId);
            if (_todo == null)
            {
                // The todo does not exist (anymore): nothing to edit, go back
                await Shell.Current.GoToAsync("..");
                return;
            }

            // Copy the model into the form fields
            Title = _todo.Title;
            Notes = _todo.Notes;
            DueDate = _todo.DueDate;
            Priority = _todo.Priority;
            IsDone = _todo.IsDone;
            SaveCommand.NotifyCanExecuteChanged();   // _todo is set now, so Save may become enabled
        }

        // Same title rule as the add page, from one shared place
        private bool CanSave() => _todo != null && TodoItem.IsValidTitle(Title);

        [RelayCommand(CanExecute = nameof(CanSave))]
        private async Task SaveAsync()
        {
            // Copy the form fields back into the model and write it with an UPDATE
            _todo!.Title = Title.Trim();
            _todo.Notes = Notes.Trim();
            _todo.DueDate = DueDate;
            _todo.Priority = (int)Priority;
            _todo.IsDone = IsDone;

            await _repository.UpdateAsync(_todo);
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task DeleteAsync()
        {
            if (_todo == null)
                return;

            await _repository.DeleteAsync(_todo);
            await Shell.Current.GoToAsync("..");
        }
    }
}
