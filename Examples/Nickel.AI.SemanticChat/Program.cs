using Microsoft.SemanticKernel;

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

#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                Kernel kernel = Kernel.CreateBuilder()
                    .AddOpenAIChatCompletion("llama3", ollamaEndpoint, null)
                    .Build();
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

                while (true)
                {
                    Console.Write("Question: ");
                    Console.WriteLine(await kernel.InvokePromptAsync(Console.ReadLine()!));
                    Console.WriteLine();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}