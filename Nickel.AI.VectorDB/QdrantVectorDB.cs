using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace Nickel.AI.VectorDB
{
    public class QdrantVectorDB : IVectorDB
    {
        private string _url;

        // NOTE: The qdrant provided client uses gRPC to connect. There is also a REST API available
        // NOTE: qdrant has more configuration options for connecting but using default for now

        public QdrantVectorDB(string url)
        {
            _url = url;
        }

        private QdrantClient GetClient()
        {
            return new QdrantClient(_url);
        }

        private Distance MapDistanceType(DistanceType distanceType)
        {
            switch (distanceType)
            {
                case DistanceType.Cosine:
                default:
                    return Distance.Cosine;
                case DistanceType.Dot:
                    return Distance.Dot;
                case DistanceType.Euclidian:
                    return Distance.Euclid;
                case DistanceType.Manhattan:
                    return Distance.Manhattan;
            }
        }

        private IDictionary<string, Value> MapToPayload(Dictionary<string, string> payload)
        {
            return payload.ToDictionary(kvp => kvp.Key, kvp => new Value(kvp.Value));
        }

        private List<PointStruct> MapToPointStructs(List<VectorPoint> points)
        {
            // NOTE: PointStruct.Payload is readonly collection property ... Can't just map with LINQ :(
            /*
            return points.Select(p => new PointStruct()
            {
                Id = new PointId() { Uuid = p.Id },
                Vectors = p.Vectors,
                Payload = MapPayload(p.Payload)
            }).ToList();
            */
            var pointStructs = new List<PointStruct>();

            foreach (var p in points)
            {
                // TODO: Enforcing Uuid here. Need to support Num
                var pointStruct = new PointStruct()
                {
                    Id = new PointId() { Uuid = p.Id },
                    Vectors = p.Vectors
                };

                if (p.Payload != null)
                {
                    pointStruct.Payload.Add(MapToPayload(p.Payload));
                }

                pointStructs.Add(pointStruct);
            }

            return pointStructs;
        }

        public async void CreateCollection(string name, ulong size, DistanceType distanceType)
        {
            var client = GetClient();
            await client.CreateCollectionAsync(name,
                new VectorParams { Size = size, Distance = MapDistanceType(distanceType) });
        }

        public async void Upsert(string collectionName, List<VectorPoint> points)
        {
            var client = GetClient();
            await client.UpsertAsync(collectionName, MapToPointStructs(points));
        }

        public List<VectorPoint> Search(string collectionName, float[] queryVector, int limit)
        {
            throw new NotImplementedException();
        }
    }
}
