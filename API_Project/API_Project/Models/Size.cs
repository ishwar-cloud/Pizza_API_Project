using System.Text.Json.Serialization;

namespace API_Project.Models
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Size
    {
        Small,
        Medium,
        Large
    }
}
