using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MPConstruction.Models
{
    class ImageResponse
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }
        [JsonPropertyName("data")]
        public List<Data> Data { get;set; }
    }

    class Data
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
