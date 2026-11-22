using System.Text.Json.Serialization;

namespace LifeManagement.Models
{
    // The "current" part of the JSON: { "temperature_2m": 0.8, "wind_speed_10m": 4.4 }
    // Keys we don't list here (time, interval) are simply ignored.
    public class OpenMeteoCurrent
    {
        [JsonPropertyName("temperature_2m")]
        public double Temperature { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public double WindSpeed { get; set; }
    }
}
