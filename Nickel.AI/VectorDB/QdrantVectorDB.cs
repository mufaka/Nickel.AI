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
            var uri = new Uri(_url);

            // client wants host and port....
            return new QdrantClient(uri.Host, uri.Port);
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

        private Dictionary<string, string> MapFromPayload(IDictionary<string, Value> payload)
        {
            return payload.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.StringValue);
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

        public async Task<List<string>> ListCollections()
        {
            var client = GetClient();
            var readOnlyList = await client.ListCollectionsAsync();
            return readOnlyList == null ? new List<string>() : [.. readOnlyList];
        }

        public async Task<bool> CollectionExists(string name)
        {
            var client = GetClient();
            return await client.CollectionExistsAsync(name);
        }

        public async Task<bool> CreateCollection(string name, ulong size, DistanceType distanceType)
        {
            var client = GetClient();

            // if the collection already exists, return.
            var exists = await client.CollectionExistsAsync(name);
            if (exists) return false;

            await client.CreateCollectionAsync(name,
                new VectorParams { Size = size, Distance = MapDistanceType(distanceType) });

            return true;
        }

        public async void Upsert(string collectionName, List<VectorPoint> points)
        {
            var client = GetClient();
            await client.UpsertAsync(collectionName, MapToPointStructs(points));
        }

        public async Task<List<VectorPoint>> Search(string collectionName, float[] queryVector, int limit)
        {
            var client = GetClient();
            var points = await client.SearchAsync(
                collectionName,
                queryVector,
                limit: (ulong)limit);

            var results = new List<VectorPoint>();

            foreach (var point in points)
            {
                results.Add(new VectorPoint()
                {
                    Id = point.Id.ToString(),
                    Payload = MapFromPayload(point.Payload),
                    Score = point.Score,
                });
            }

            return results;
        }
    }
}
