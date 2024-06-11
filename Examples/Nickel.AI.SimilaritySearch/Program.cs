using Nickel.AI.Embeddings;
using Nickel.AI.VectorDB;

namespace Nickel.AI.SimilaritySearch
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IVectorDB qdrant = new QdrantVectorDB("http://localhost:6334");

            // NOTE: qdrant allows for adding the documents directly to a collection, in a batch, without providing embeddings but not going to use that.
            // embed the documents in qdrant.
            //var embedder = new OllamaEmbedder("http://localhost:11434", "mxbai-embed-large");
            var embedder = new OllamaEmbedder("http://localhost:11434", "llama3");

            var collections = new List<(string, DistanceType)>()
            {
                ("knowledge_base_cos", DistanceType.Cosine),
                ("knowledge_base_euc", DistanceType.Euclidian),
                ("knowledge_base_man", DistanceType.Manhattan),
                ("knowledge_base_dot", DistanceType.Dot)
            };

            foreach (var collection in collections)
            {
                await CreateKnowledgebase(collection.Item1, collection.Item2, qdrant, embedder);
            }

            var prompt = "What tools should I need to use to build a web service using vector embeddings for search?";

            // find similar content in qdrant. get embedding for prompt, search qdrant
            var promptEmbedding = await embedder.GetEmbedding(prompt);
            var promptEmbeddingVector = promptEmbedding.Select(f => (float)f).ToArray();

            foreach (var collection in collections)
            {
                var results = await qdrant.Search(collection.Item1, promptEmbeddingVector, 3);

                Console.WriteLine($"Results for: '{collection.Item1}'");
                Console.WriteLine();

                if (results != null)
                {
                    foreach (var result in results)
                    {
                        Console.WriteLine($"ID: {result.Id} SCORE: {result.Score}");
                        Console.WriteLine(result.Payload?["document"]);
                        Console.WriteLine();
                    }
                }
            }
        }

        private static async Task CreateKnowledgebase(string collectionName, DistanceType distanceType, IVectorDB qdrant, OllamaEmbedder embedder)
        {
            // NOTE: because we are using new guids for the points, re-running this will create dupes. Just bail if collection
            //       exists.
            var collectionExists = await qdrant.CollectionExists(collectionName);
            if (collectionExists) return;

            var documents = new string[] {
                "Qdrant is a vector database & vector similarity search engine. It deploys as an API service providing search for the nearest high-dimensional vectors. With Qdrant, embeddings or neural network encoders can be turned into full-fledged applications for matching, searching, recommending, and much more!",
                "Docker helps developers build, share, and run applications anywhere — without tedious environment configuration or management.",
                "PyTorch is a machine learning framework based on the Torch library, used for applications such as computer vision and natural language processing.",
                "MySQL is an open-source relational database management system (RDBMS). A relational database organizes data into one or more data tables in which data may be related to each other; these relations help structure the data. SQL is a language that programmers use to create, modify and extract data from the relational database, as well as control user access to the database.",
                "NGINX is a free, open-source, high-performance HTTP server and reverse proxy, as well as an IMAP/POP3 proxy server. NGINX is known for its high performance, stability, rich feature set, simple configuration, and low resource consumption.",
                "FastAPI is a modern, fast (high-performance), web framework for building APIs with Python 3.7+ based on standard Python type hints.",
                "SentenceTransformers is a Python framework for state-of-the-art sentence, text and image embeddings. You can use this framework to compute sentence / text embeddings for more than 100 languages. These embeddings can then be compared e.g. with cosine-similarity to find sentences with a similar meaning. This can be useful for semantic textual similar, semantic search, or paraphrase mining.",
                "The cron command-line utility is a job scheduler on Unix-like operating systems. Users who set up and maintain software environments use cron to schedule jobs (commands or shell scripts), also known as cron jobs, to run periodically at fixed times, dates, or intervals." };

            // TODO: double check the size parameter.
            // create a collection, use Cosine
            qdrant.CreateCollection(collectionName, 4096, distanceType);

            // create vector points from documents
            List<VectorPoint> points = new List<VectorPoint>();

            foreach (var document in documents)
            {
                var point = new VectorPoint();

                point.Id = Guid.NewGuid().ToString();

                // TODO: embedder uses double[], qdrant client uses float[]. Casting here for now but this sucks because it
                //       could barf (64 bit to 32 bit ..)
                var embeddings = await embedder.GetEmbedding(document);
                point.Vectors = embeddings.Select(f => (float)f).ToArray();
                point.Payload = new Dictionary<string, string>()
                {
                    { "document", document }
                };

                points.Add(point);
            }

            // upsert points to qdrant
            qdrant.Upsert(collectionName, points);
        }

    }
}
