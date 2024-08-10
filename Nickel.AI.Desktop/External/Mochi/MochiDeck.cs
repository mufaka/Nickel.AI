using System.Text.Json.Serialization;

namespace Nickel.AI.Desktop.External.Mochi
{
    public class MochiDeck
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("parent-id")]
        public string ParentId { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("sort")]
        public int Sort { get; set; }
    }

    public class MochiDeckListResponse
    {
        [JsonPropertyName("bookmark")]
        public string Bookmark { get; set; } = string.Empty;

        [JsonPropertyName("docs")]
        public List<MochiDeck> Decks { get; set; } = new List<MochiDeck>();
    }
}
