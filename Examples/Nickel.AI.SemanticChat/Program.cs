#pragma warning disable SKEXP0010
using OllamaSharp;
using OllamaSharp.Models;
using System.Text;

namespace Nickel.AI.SemanticChat
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var ollamaEndpoint = new Uri("http://localhost:11434");

                var ollama = new OllamaApiClient(ollamaEndpoint);

                while (true)
                {
                    Console.Write("Question: ");
                    var message = ReadInput();

                    if (!String.IsNullOrWhiteSpace(message))
                    {
                        var completionRequest = new GenerateCompletionRequest();
                        completionRequest.Stream = false;
                        completionRequest.Prompt = message;
                        completionRequest.Model = "llama3";

                        var completionResponse = await ollama.GetCompletion(completionRequest);
                        Console.WriteLine(completionResponse.Response);
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

        private static string ReadInput()
        {
            var buff = new StringBuilder();

            var input = Console.ReadLine();

            while (input != "END")
            {
                buff.AppendLine(input);
                input = Console.ReadLine();
            }

            return buff.ToString();
        }
    }
}