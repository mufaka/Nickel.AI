namespace Nickel.AI.VectorDB
{
    // NOTE: This will grow/change over time. Starting with basic functionality needed for RAG and qdrant

    public interface IVectorDB
    {
        void CreateCollection(string name, ulong size, DistanceType distanceType);
        void Upsert(string collectionName, List<VectorPoint> points);
        Task<List<VectorPoint>> Search(string collectionName, float[] queryVector, int limit);
    }
}
