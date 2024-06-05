﻿using Nickel.AI.Extraction;

namespace Nickel.AI.TextExtraction
{
    internal class Program
    {
        class Options
        {
            public bool ShowHelp { get; set; } = false;
            public string HelpMessage { get; set; } = string.Empty;
            public string UriPath { get; set; } = string.Empty;
        }

        static void Main(string[] args)
        {
            try
            {
                var options = ParseCommandLine(args);

                if (options.ShowHelp)
                {
                    PrintUsage(options);
                    return;
                }

                var extractor = new TextExtractor();
                var extractedDocument = extractor.Extract(new Uri(options.UriPath));

                Console.WriteLine(extractedDocument);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static Options ParseCommandLine(string[] args)
        {
            Options options = new Options();

            if (args.Length != 1)
            {
                options.ShowHelp = true;
                options.HelpMessage = "The number of arguments is incorrect.";
            }
            else
            {
                options.UriPath = args[0];

                try
                {
                    var uri = new Uri(options.UriPath);

                    if (uri.Scheme == "file" && !File.Exists(options.UriPath))
                    {
                        options.ShowHelp = true;
                        options.HelpMessage = $"Unable to find {options.UriPath}.";
                    }
                }
                catch (Exception ex)
                {
                    options.ShowHelp = true;
                    options.HelpMessage = ex.Message;
                }

            }

            return options;
        }

        static void PrintUsage(Options options)
        {
            if (!String.IsNullOrEmpty(options.HelpMessage))
            {
                Console.WriteLine(options.HelpMessage);
                Console.WriteLine();
            }

            Console.WriteLine("dotnet Nickel.AI.TextExtractor <file>");
            Console.WriteLine("\t <uri> - The URI to extract text from.");
        }
    }
}
