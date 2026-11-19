namespace LifeManagement.Models
{
    // The weather the app needs, in our own simple shape.
    // The service fills it from Open-Meteo's JSON, so the rest of the app never sees the JSON.
    public class WeatherInfo
    {
        public double Temperature { get; set; }   // °C
        public double WindSpeed { get; set; }     // m/s (we ask the API for m/s)
    }
}
