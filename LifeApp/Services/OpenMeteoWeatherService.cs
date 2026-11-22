using System.Net.Http.Json;
using LifeManagement.Models;

namespace LifeManagement.Services
{
    public class OpenMeteoWeatherService : IWeatherService
    {
        // Vaasa. wind_speed_unit=ms is needed: without it Open-Meteo answers in km/h.
        private const string Url =
            "https://api.open-meteo.com/v1/forecast?latitude=63.10&longitude=21.62" +
            "&current=temperature_2m,wind_speed_10m&wind_speed_unit=ms";

        private readonly HttpClient _http;

        // DI passes in the one shared HttpClient (registered in MauiProgram)
        public OpenMeteoWeatherService(HttpClient http)
        {
            _http = http;
        }

        public async Task<WeatherInfo?> GetCurrentWeatherAsync()
        {
            try
            {
                // Downloads the JSON and turns it into an OpenMeteoResponse object in one step
                var response = await _http.GetFromJsonAsync<OpenMeteoResponse>(Url);
                if (response?.Current == null)
                    return null;

                return new WeatherInfo
                {
                    Temperature = response.Current.Temperature,
                    WindSpeed = response.Current.WindSpeed
                };
            }
            catch (Exception)
            {
                // No network, timeout or unexpected JSON: the app must not crash.
                // null tells the ViewModel to show a message instead of the weather.
                return null;
            }
        }
    }
}
