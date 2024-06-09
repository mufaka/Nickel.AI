namespace Nickel.AI.Embeddings
{
    public interface IEmbedder
    {
        Task<double[]> GetEmbedding(string words);
    }
}
