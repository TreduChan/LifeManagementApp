using LifeManagement.Models;

namespace LifeManagement.Services
{
    public interface IWeatherService
    {
        // Returns null when the weather could not be loaded (no network, server error)
        Task<WeatherInfo?> GetCurrentWeatherAsync();
    }
}
