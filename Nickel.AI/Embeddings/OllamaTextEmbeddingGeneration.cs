#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;
using OllamaSharp;

namespace Nickel.AI.Embeddings
{
    /// <summary>
    /// A SemanticKernel ITextEmbeddingGenerationService implementation that works with Ollama. The existing
    /// OpenAITextEmbeddingGeneration doesn't support providing a URL so this will do the job instead.
    /// </summary>
    public class OllamaTextEmbeddingGeneration : ITextEmbeddingGenerationService
    {
        private OllamaApiClient? _ollamaApiClient;

        // NOTE: I don't think these need to be implemented.
        private static IReadOnlyDictionary<string, object?> _attributes = new Dictionary<string, object?>();

        // NOTE: order of parameters following what appears to be SK convention
        public OllamaTextEmbeddingGeneration(string modelName, string endPointUrl)
        {
            _ollamaApiClient = new OllamaApiClient(endPointUrl, modelName);
        }

        public IReadOnlyDictionary<string, object?> Attributes
        {
            get
            {
                return _attributes;
            }
        }

        public async Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data, Kernel? kernel = null, CancellationToken cancellationToken = default)
        {
            var results = new List<ReadOnlyMemory<float>>();

            foreach (var text in data)
            {
                var response = await _ollamaApiClient!.GenerateEmbeddings(text);
                results.Add(new ReadOnlyMemory<float>(response.Embedding.Select(f => (float)f).ToArray()));
            }

            return results;
        }
    }
}
