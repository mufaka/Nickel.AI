using System.Text.Json.Serialization;

namespace Nickel.AI.Desktop.External.Mochi
{
    public class MochiCard
    {
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("deck-id")]
        public string DeckId { get; set; } = string.Empty;
    }
}
