using System.Text.Json.Serialization;

namespace Barber.UI.Entities
{
    public class SchedulesResponse
    {
        public string Id { get; set; }
        [JsonPropertyName("$values")]
        public List<SchedulesDTO> Values { get; set; }
    }
}
