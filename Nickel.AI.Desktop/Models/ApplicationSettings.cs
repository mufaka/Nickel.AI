namespace Nickel.AI.Desktop.Models
{
    public class ApplicationSettings
    {
        public string Theme { get; set; } = "Moonlight";
        public OllamaSettings Ollama { get; set; } = new OllamaSettings();
        public QdrantSettings Qdrant { get; set; } = new QdrantSettings();
    }
}
