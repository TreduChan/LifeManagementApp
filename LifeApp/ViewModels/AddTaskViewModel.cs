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
        // ---------- categories (part 3 changes this line!) ----------
        public List<string> Categories { get; } = new() { "Study", "Home", "Health", "Work" };

        // ---------- form fields ----------
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TitleCounter))]   // the counter label updates too
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
    }
}
