using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;

namespace LifeManagement
{
    public class AddTaskForm : INotifyPropertyChanged
    {
        // ---------- plumbing (given – lecture slides 24–25) ----------
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }

        // ---------- data shown on the page ----------
        public List<string> Categories { get; } = new() { "Study", "Home", "Health", "Work" };

        private string title = "";
        public string Title                                  // example – given
        {
            get => title;
            set
            {
                if (Set(ref title, value ?? ""))
                {
                    OnPropertyChanged(nameof(TitleCounter)); // computed property below
                    SaveCommand.ChangeCanExecute();          // 4a: re-check CanSave
                }
            }
        }

        public string TitleCounter => $"{Title.Length}/40";

        // 4b: the other form fields
        private string notes = "";
        public string Notes
        {
            get => notes;
            set => Set(ref notes, value);
        }

        private string category = "Study";
        public string Category
        {
            get => category;
            set => Set(ref category, value);
        }

        private DateTime dueDate = DateTime.Today.AddDays(1);
        public DateTime DueDate
        {
            get => dueDate;
            set => Set(ref dueDate, value);
        }

        private double priority = 3;
        public double Priority
        {
            get => priority;
            set => Set(ref priority, value);
        }

        private bool reminder = true;
        public bool Reminder
        {
            get => reminder;
            set => Set(ref reminder, value);
        }

        private string result = "";
        public string Result
        {
            get => result;
            set => Set(ref result, value);
        }

        // ---------- the action ----------
        public Command SaveCommand { get; }

        public AddTaskForm()
        {
            SaveCommand = new Command(Save, CanSave);        // 4c
        }

        private bool CanSave()
        {
            // 4d: not empty after trimming, and at most 40 characters
            return !string.IsNullOrWhiteSpace(Title) && Title.Length <= 40;
        }

        private void Save()
        {
            // 4e: same summary line as before
            Result = $"Saved: {Title.Trim()} · {Category} · P{(int)Priority} · due {DueDate:dd.MM.yyyy} · reminder {(Reminder ? "on" : "off")}";
        }
    }
}
