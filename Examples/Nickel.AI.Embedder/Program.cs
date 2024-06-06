using Nickel.AI.Embeddings;

namespace Nickel.AI.Embedder
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var embedder = new OllamaEmbedder("http://localhost:11434", "mxbai-embed-large");
                // var embedder = new OllamaEmbedder("http://localhost:11434", "llama3");
                var embeddings = await embedder.GetEmbedding("Codet5 is based on Google’s T5 framework but incorporates better code-specific knowledge. It can perform operations like code completion, summarization, and translation between different programming languages. Despite its capabilities, Codet5 is not as widely available as other AI programming tools like GitHub Copilot or OpenAI Codex.");

                Console.WriteLine(String.Join(',', embeddings));
                Console.WriteLine();
                Console.WriteLine($"Embedding count: {embeddings.Length}");


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
