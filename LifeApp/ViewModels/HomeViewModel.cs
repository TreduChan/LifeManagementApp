using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LifeManagement.Models;
using LifeManagement.Services;

namespace LifeManagement.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        // Wind above this (m/s) counts as kitesurf weather
        private const double KitesurfWindLimit = 5;

        private readonly ITodoRepository _repository;
        private readonly IWeatherService _weatherService;

        // Two services from DI: one for the database, one for the weather API
        public HomeViewModel(ITodoRepository repository, IWeatherService weatherService)
        {
            _repository = repository;
            _weatherService = weatherService;
        }

        public ObservableCollection<TodoItem> TopTodos { get; } = new();

        [ObservableProperty] private string weatherText = "Loading weather...";
        [ObservableProperty] private bool isKitesurfWeather;

        // The top 3 rule in one place. static + no database = easy to unit test (step 5).
        public static List<TodoItem> PickTop3(IEnumerable<TodoItem> todos)
        {
            return todos
                .Where(t => !t.IsDone)                 // only unfinished
                .OrderByDescending(t => t.Priority)    // priority 5 first
                .ThenBy(t => t.DueDate)                // same priority: earliest due date first
                .Take(3)
                .ToList();
        }

        // Called by the page every time it appears
        public async Task LoadAsync()
        {
            // ---------- top 3 todos ----------
            var all = await _repository.GetAllAsync();
            TopTodos.Clear();
            foreach (var todo in PickTop3(all))
                TopTodos.Add(todo);

            // ---------- weather ----------
            WeatherText = "Loading weather...";
            var weather = await _weatherService.GetCurrentWeatherAsync();

            if (weather == null)
            {
                // The service returned null: no network or the API failed
                WeatherText = "Weather not available. Check your internet connection.";
                IsKitesurfWeather = false;
                return;
            }

            WeatherText = $"Vaasa: {weather.Temperature:0.0} °C, wind {weather.WindSpeed:0.0} m/s";
            IsKitesurfWeather = weather.WindSpeed > KitesurfWindLimit;
        }
    }
}
