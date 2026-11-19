using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace LifeManagement.ViewModels
{
    public partial class AddTaskViewModel : ObservableObject
    {
        // ---------- categories: ObservableCollection tells the Picker when an item is added ----------
        public ObservableCollection<string> Categories { get; } = new() { "Study", "Home", "Health", "Work" };

        // ---------- form fields ----------
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TitleCounter))]   // the counter label updates too
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]   // Save re-checks CanSave
        private string title = "";                         // example – given

        [ObservableProperty]
        private string notes = "";

        [ObservableProperty]
        private string category = "Study";

        [ObservableProperty]
        private DateTime dueDate = DateTime.Today.AddDays(1);

        [ObservableProperty]
        private double priority = 3;

        [ObservableProperty]
        private bool reminder = true;

        [ObservableProperty]
        private string result = "";

        public string TitleCounter => $"{Title?.Length ?? 0}/40";

        private bool CanSave()
        {
            // not empty after trimming, and at most 40 characters
            return !string.IsNullOrWhiteSpace(Title) && Title.Length <= 40;
        }

        // ---------- new category ----------
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddCategoryCommand))]
        private string newCategory = "";

        private bool CanAddCategory()
        {
            // not empty after trimming, and not already in the list (ignore case)
            string name = NewCategory?.Trim() ?? "";
            return name.Length > 0
                && !Categories.Any(c => string.Equals(c, name, StringComparison.OrdinalIgnoreCase));
        }

        [RelayCommand(CanExecute = nameof(CanAddCategory))]
        private void AddCategory()                // -> AddCategoryCommand
        {
            string name = NewCategory.Trim();
            Categories.Add(name);
            Category = name;      // select the new category
            NewCategory = "";
        }

        [RelayCommand(CanExecute = nameof(CanSave))]
        private void Save()                       // -> the generator creates SaveCommand
        {
            Result = $"Saved: {Title.Trim()} · {Category} · P{(int)Priority} · due {DueDate:dd.MM.yyyy} · reminder {(Reminder ? "on" : "off")}";
        }
    }
}
