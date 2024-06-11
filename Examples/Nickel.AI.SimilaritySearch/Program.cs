using Nickel.AI.Embeddings;
using Nickel.AI.VectorDB;
using OllamaSharp;
using OllamaSharp.Models;

namespace Nickel.AI.SimilaritySearch
{
    internal class Program
    {
        // TODO: this example really is RAG, not similarity search.
        // NOTE: wwdc-2024.txt is copy / pasted from https://techcrunch.com/2024/06/10/everything-apple-announced-wwdc-2024/

        static async Task Main(string[] args)
        {
            // Adapted from https://github.com/qdrant/examples/blob/master/rag-openai-qdrant/rag-openai-qdrant.ipynb
            const string collectionName = "knowledge_base";
            IVectorDB qdrant = new QdrantVectorDB("http://localhost:6334");

            // IMPORTANT: This is slow. Because we are using the same instance of Ollama at localhost with two different
            //            models (mxbai-embed-large for embedding, llama3 for chat completion), we deal with cold startup
            //            type latency because Ollama has to switch models. We can use llama3 for embedding as well but
            //            that vector size is 4096. Ideally the embedding endpoint would be separate from the llm endpoint.

            // NOTE: qdrant allows for adding the documents directly to a collection, in a batch, without providing embeddings but not going to use that.
            // embed the documents in qdrant. The client uses gRPC on port 6334
            var embedder = new OllamaEmbedder("http://localhost:11434", "mxbai-embed-large");

            await CreateKnowledgebase(collectionName, qdrant, embedder);

            var prompt = "What tools should I need to use to build a web service using vector embeddings for search?";
            var ollamaEndpoint = new Uri("http://localhost:11434");
            var ollama = new OllamaApiClient(ollamaEndpoint);

            // ask without giving a context
            var completionRequest = new GenerateCompletionRequest();
            completionRequest.Stream = false;
            completionRequest.Prompt = prompt;
            completionRequest.Model = "llama3";

            var completionResponse = await ollama.GetCompletion(completionRequest);
            Console.WriteLine(completionResponse.Response);
            Console.WriteLine();

            // find similar content in qdrant. get embedding for prompt, search qdrant
            var promptEmbedding = await embedder.GetEmbedding(prompt);
            var results = await qdrant.Search(collectionName, promptEmbedding.Select(f => (float)f).ToArray(), 3);

            if (results != null)
            {
                var context = String.Join("\n", results.Select(r => r.Payload?["text"]));

                var ragPrompt = @$"You are a software architect. 
Answer the following question using the provided context. 
If you can't find the answer, do not pretend you know it, but answer ""I don't know"".

Question: {prompt.Trim()}

Context:
{context.Trim()}

Answer:
";
                Console.WriteLine("RAG Prompt");
                Console.WriteLine(ragPrompt);
                Console.WriteLine();

                Console.WriteLine("RAG Response");
                completionRequest.Prompt = ragPrompt;
                completionResponse = await ollama.GetCompletion(completionRequest);
                Console.WriteLine(completionResponse.Response);
                Console.WriteLine();
            }
        }

        private static async Task CreateKnowledgebase(string collectionName, IVectorDB qdrant, OllamaEmbedder embedder)
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
            qdrant.CreateCollection(collectionName, 1024, DistanceType.Cosine);

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
                    { "text", document }
                };

                points.Add(point);
            }

            // upsert points to qdrant
            qdrant.Upsert(collectionName, points);
        }

        /* -- No context response
To build a web service using vector embeddings for search, you'll need a combination of programming languages, frameworks, and libraries. Here's a suggested toolkit:

1. **Programming language**:
        * Python is a popular choice for building search services, especially with libraries like TensorFlow or scikit-learn.
        * Java or C# can also be used, depending on your team's expertise and the specific requirements of your service.
2. **Vector embedding library**:
        * TensorFlow (Python) or TensorFlow.js (JavaScript) provide excellent support for vector embeddings using techniques like Word2Vec or GloVe.
        * scikit-learn (Python) has a built-in implementation of Word2Vec, which can be used as a starting point.
3. **Search framework**:
        * Elasticsearch (Java-based) is a popular choice for building search services. It provides a robust indexing and querying mechanism that can integrate with your vector embeddings.
        * Apache Solr (Java-based) is another widely-used search engine that supports vector searches.
4. **Indexing and caching**:
        * Redis or Memcached can be used as an in-memory data store to cache frequently accessed data, such as indexed documents or query results.
5. **API framework**:
        * Flask (Python) or Django (Python) are popular web frameworks for building RESTful APIs.
        * Java-based frameworks like Spring Boot or Jersey can also be used.
6. **Natural Language Processing (NLP) libraries**:
        * NLTK (Python) or Stanford CoreNLP (Java) provide tools for tokenization, stemming, and lemmatizing text data.
7. **Data processing and storage**:
        * Apache Hadoop or Spark can be used for large-scale data processing and storage.

To get started, you may want to focus on the following core components:

1. Vector embedding library (e.g., TensorFlow or scikit-learn)
2. Search framework (e.g., Elasticsearch or Apache Solr)
3. API framework (e.g., Flask or Django)

As you progress with your project, you can integrate additional tools and libraries as needed to enhance performance, scalability, and features.

Keep in mind that the specific requirements of your service may vary depending on factors like:

* The type of data you're working with (text, images, etc.)
* The size and complexity of your dataset
* The desired level of accuracy and speed for search results

Remember to carefully evaluate the trade-offs between these components and consider factors like performance, scalability, and maintainability as you design your web service.
    */
        /*
        RAG Response
        Based on the provided context, I would recommend the following tools to build a web service using vector embeddings for search:

        * FastAPI: As it's mentioned in the context, FastAPI is a suitable framework for building APIs with Python 3.7+. It provides high-performance and is well-suited for building RESTful APIs.
        * PyTorch: Since you want to use vector embeddings for search, you'll likely need a machine learning library like PyTorch to train and generate these embeddings.
        * Docker: As mentioned in the context, Docker can help with environment configuration and management. It's often useful when working with machine learning or AI-related projects that require specific dependencies.

        Additionally, you may also want to consider other tools such as:

        * A database (e.g., PostgreSQL, MongoDB) to store your search data
        * A vector processing library like OpenCV or TensorFlow for computing similarities between vectors
        * A programming language like Python (since FastAPI and PyTorch are both written in Python) or R for scripting and data manipulation
        * A visualization tool like Matplotlib or Plotly to visualize the results of your search

        I hope this helps!
        */
    }
}
