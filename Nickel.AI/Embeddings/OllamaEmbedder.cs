using OllamaSharp;

namespace Nickel.AI.Embeddings
{
    public class OllamaEmbedder : IEmbedder
    {
        private string _endPointUrl;
        private string _modelName;

        public OllamaEmbedder(string endPointUrl, string modelName)
        {
            _endPointUrl = endPointUrl;
            _modelName = modelName;
        }

        public async Task<double[]> GetEmbedding(string words)
        {
            var ollamaEndpoint = new Uri(_endPointUrl);
            var ollama = new OllamaApiClient(ollamaEndpoint);
            ollama.SelectedModel = _modelName;

            var response = await ollama.GenerateEmbeddings(words);

            return response.Embedding;
        }
    }
}
