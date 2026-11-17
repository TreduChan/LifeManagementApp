using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace LifeManagement
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();   // controls exist only after this line

            // TODO 1 + 2: categories, first one selected
            CategoryPicker.ItemsSource = new List<string> { "Study", "Home", "Health", "Work" };
            CategoryPicker.SelectedIndex = 0;

            // TODO 3: due date defaults to tomorrow
            DueDatePicker.Date = DateTime.Today.AddDays(1);
        }

        // Keeps the number next to the Stepper in sync with its value
        private void OnPriorityChanged(object? sender, ValueChangedEventArgs e)
        {
            PriorityLabel.Text = ((int)e.NewValue).ToString();
        }

        private void OnSaveClicked(object? sender, EventArgs e)
        {
            // TODO 4: an empty title is not accepted
            string title = TitleEntry.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(title))
            {
                ResultLabel.Text = "Title is required.";
                ResultLabel.TextColor = Colors.Red;
                return;
            }

            // TODO 5: read the other values
            string category = CategoryPicker.SelectedItem as string ?? "-";
            int priority = (int)PriorityStepper.Value;   // Value is a double
            DateTime due = DueDatePicker.Date;
            bool reminder = ReminderSwitch.IsToggled;

            // TODO 6: summary in green
            string summary = $"{title} · {category} · P{priority} · due {due:dd.MM.yyyy} · reminder {(reminder ? "on" : "off")}";
            ResultLabel.Text = $"Saved: {summary}";
            ResultLabel.TextColor = Colors.Green;

            // Saved cards: new card at the top of the list, built in C# instead of XAML
            SavedList.Children.Insert(0, new Border
            {
                Padding = 10,
                Content = new Label { Text = summary }
            });
        }
    }
}
