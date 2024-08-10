using System.Text.Json.Serialization;

namespace Nickel.AI.Desktop.External.Mochi
{
    public class MochiDeck
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("sort")]
        public int Sort { get; set; }
    }

    public class MochiDeckListResponse
    {
        [JsonPropertyName("bookmark")]
        public string Bookmark { get; set; }

        [JsonPropertyName("docs")]
        public List<MochiDeck> Decks { get; set; } = new List<MochiDeck>();
    }
}
