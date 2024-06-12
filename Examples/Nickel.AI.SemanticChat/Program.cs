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
                    var prompt = Console.ReadLine();

                    if (!String.IsNullOrWhiteSpace(prompt))
                    {
                        Console.WriteLine(await kernel.InvokePromptAsync(prompt));
                        Console.WriteLine();
                    }
                    else
                    {
                        break;
                    }
                }

                // Do kernel functions work with Ollama?
                kernel.ImportPluginFromFunctions("DateTimeHelpers",
                [
                    kernel.CreateFunctionFromMethod(() => $"{DateTime.Now:r}", "Now", "Gets the current date and time")
                ]);

                KernelFunction qa = kernel.CreateFunctionFromPrompt("""
    The current date and time is {{ datetimehelpers.now }}.
    {{ $input }}
    """);
                var arguments = new KernelArguments();
                arguments["input"] = "What time is it?";

                Console.WriteLine(await qa.InvokeAsync(kernel, arguments));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}