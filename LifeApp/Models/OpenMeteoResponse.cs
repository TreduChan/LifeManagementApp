using System.Text.Json.Serialization;

namespace LifeManagement.Models
{
    // Shaped like Open-Meteo's JSON answer: { "current": { ... } }
    // Only the weather service uses it; the rest of the app uses WeatherInfo.
    public class OpenMeteoResponse
    {
        // [JsonPropertyName] connects the JSON key "current" to this property
        [JsonPropertyName("current")]
        public OpenMeteoCurrent? Current { get; set; }
    }
}
