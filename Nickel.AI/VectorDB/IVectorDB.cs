namespace Nickel.AI.VectorDB
{
    // NOTE: This will grow/change over time. Starting with basic functionality needed for RAG and qdrant
    //       which means we are only going to be indexing text in the DB.

    public interface IVectorDB
    {
        Task<bool> CollectionExists(string name);
        Task<bool> CreateCollection(string name, ulong size, DistanceType distanceType);
        void Upsert(string collectionName, List<VectorPoint> points);
        Task<List<VectorPoint>> Search(string collectionName, float[] queryVector, int limit);
        Task<List<string>> ListCollections();
    }
}
