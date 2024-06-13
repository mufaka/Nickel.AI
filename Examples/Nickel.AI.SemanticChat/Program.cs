#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0020 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Microsoft.SemanticKernel.Memory;
using Nickel.AI.Embeddings;
using System.Text;

namespace Nickel.AI.SemanticChat
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                // Using local Ollama with .AddOpenAIChatCompletion works! It supports, enough of, the OpenAI API. 
                var ollamaEndpoint = new Uri("http://localhost:11434");

                var builder = Kernel.CreateBuilder();
                builder.AddOpenAIChatCompletion("llama3", ollamaEndpoint, null);
                builder.Services.AddLogging(c => c.AddConsole().SetMinimumLevel(LogLevel.Warning));

                var kernel = builder.Build();

                ISemanticTextMemory memory = new MemoryBuilder()
                    .WithLoggerFactory(kernel.LoggerFactory)
                    .WithQdrantMemoryStore("http://localhost:6333/", 4096)
                    .WithTextEmbeddingGeneration(new OllamaTextEmbeddingGeneration("llama3", "http://localhost:11434"))
                    .Build();

                var collectionName = "knowledge_base_sk";

                await CreateKnowledgeBase(collectionName, memory);

                while (true)
                {
                    Console.Write("Question: ");
                    var prompt = Console.ReadLine();

                    if (!String.IsNullOrWhiteSpace(prompt))
                    {
                        // get "memory" related to given question
                        var contextBuff = new StringBuilder();
                        await foreach (var result in memory.SearchAsync(collectionName, prompt, limit: 3))
                            contextBuff.AppendLine(result.Metadata.Text);

                        var ragPrompt = $@"Answer the following question including the provided context. If you can't answer the question, do not pretend you know it, but answer ""I don't know"".
    
Question: {prompt.Trim()}

Context:
{contextBuff.ToString().Trim()}";

                        Console.WriteLine(ragPrompt);

                        Console.WriteLine(await kernel.InvokePromptAsync(ragPrompt));
                        Console.WriteLine();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        // NOTE: IMPORTANT! Even though this knowledge base is the same as the BasicRAG example and Ollama embeddings are used, SemanticKernel seems to 
        //       be embedding with different vectors. Trying to use this with BasicRAG collections yields no results on search.

        private static async Task CreateKnowledgeBase(string collectionName, ISemanticTextMemory memory)
        {
            IList<string> collections = await memory.GetCollectionsAsync();

            if (collections.Contains(collectionName))
            {
                return;
            }

            var documents = new string[] {
                "Qdrant is a vector database & vector similarity search engine. It deploys as an API service providing search for the nearest high-dimensional vectors. With Qdrant, embeddings or neural network encoders can be turned into full-fledged applications for matching, searching, recommending, and much more!",
                "Docker helps developers build, share, and run applications anywhere — without tedious environment configuration or management.",
                "PyTorch is a machine learning framework based on the Torch library, used for applications such as computer vision and natural language processing.",
                "MySQL is an open-source relational database management system (RDBMS). A relational database organizes data into one or more data tables in which data may be related to each other; these relations help structure the data. SQL is a language that programmers use to create, modify and extract data from the relational database, as well as control user access to the database.",
                "NGINX is a free, open-source, high-performance HTTP server and reverse proxy, as well as an IMAP/POP3 proxy server. NGINX is known for its high performance, stability, rich feature set, simple configuration, and low resource consumption.",
                "FastAPI is a modern, fast (high-performance), web framework for building APIs with Python 3.7+ based on standard Python type hints.",
                "SentenceTransformers is a Python framework for state-of-the-art sentence, text and image embeddings. You can use this framework to compute sentence / text embeddings for more than 100 languages. These embeddings can then be compared e.g. with cosine-similarity to find sentences with a similar meaning. This can be useful for semantic textual similar, semantic search, or paraphrase mining.",
                "The cron command-line utility is a job scheduler on Unix-like operating systems. Users who set up and maintain software environments use cron to schedule jobs (commands or shell scripts), also known as cron jobs, to run periodically at fixed times, dates, or intervals." };

            foreach (var document in documents)
            {
                await memory.SaveInformationAsync(collectionName, document, Guid.NewGuid().ToString());
            }
        }
    }
}