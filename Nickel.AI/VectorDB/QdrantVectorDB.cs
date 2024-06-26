﻿using Qdrant.Client;
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

        public async Task<bool> CollectionExists(string name)
        {
            var client = GetClient();
            return await client.CollectionExistsAsync(name);
        }

        public async void CreateCollection(string name, ulong size, DistanceType distanceType)
        {
            var client = GetClient();

            // if the collection already exists, return.
            var exists = await client.CollectionExistsAsync(name);
            if (exists) return;

            await client.CreateCollectionAsync(name,
                new VectorParams { Size = size, Distance = MapDistanceType(distanceType) });
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

            // TODO: results contain a score:
            /*
                { 
                    "id": { "uuid": "571c12de-1005-4616-b455-68d42087f8af" }, 
                    "payload": { 
                        "text": { 
                            "stringValue": "FastAPI is a modern, fast (high-performance), web framework for building APIs with Python 3.7+ based on standard Python type hints." 
                        } 
                    }, 
                    "score": 0.597257, 
                    "version": "2" 
                }
            */

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
