using Nickel.AI.Data;

namespace Nickel.AI.CsvLoader
{
    internal class Program
    {
        class Options
        {
            public bool ShowHelp { get; set; } = false;
            public string HelpMessage { get; set; } = string.Empty;
            public string CsvFile { get; set; } = string.Empty;
            public string StoragePath { get; set; } = string.Empty;
            public int FrameSize { get; set; }
        }

        static void Main(string[] args)
        {
            var options = ParseCommandLine(args);

            if (options.ShowHelp)
            {
                PrintUsage(options);
                return;
            }

            var loader = new CsvDataLoader(options.CsvFile, true);
            var storage = new CsvDataFrameStorage(options.StoragePath);
            storage.FrameSize = options.FrameSize;

            var chunkedDataInitializer = new ChunkedData();

            // chunk the data and persist
            chunkedDataInitializer.Initialize(loader, storage);

            // access the chunked data that has already been initialized
            var chunkedDataLoader = new ChunkedData();
            chunkedDataLoader.Load(storage);

            int count = 1;
            foreach (ChunkedDataFrame frame in chunkedDataLoader.Frames)
            {
                // accessing the Data property loads the data from the file system
                // so be sure to call Unload after use...
                Console.WriteLine($"{frame.FileName} {count} has {frame.Data!.Rows.Count} rows");
                frame.Unload();
                count++;
            }
        }

        static Options ParseCommandLine(string[] args)
        {
            Options options = new Options();

            if (args.Length != 3)
            {
                options.ShowHelp = true;
                options.HelpMessage = "The number of arguments is incorrect.";
            }
            else
            {
                options.CsvFile = args[0];
                options.StoragePath = args[1];

                if (!File.Exists(options.CsvFile))
                {
                    options.ShowHelp = true;
                    options.HelpMessage = $"Unable to find {options.CsvFile}.";
                }

                int frameSize;

                if (!Int32.TryParse(args[2], out frameSize))
                {
                    options.ShowHelp = true;
                    options.HelpMessage = "Invalid Frame Size.";
                }
                else
                {
                    if (frameSize <= 0)
                    {
                        options.ShowHelp = true;
                        options.HelpMessage = "Invalid Frame Size.";
                    }
                    else
                    {
                        options.FrameSize = frameSize;
                    }
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

            Console.WriteLine("dotnet Nickel.AI.CsvLoader <csv file> <storage path> <frame size>");
            Console.WriteLine("\t <csv file> - The csv file to load");
            Console.WriteLine("\t <storage path> - The root folder to store chunked data in.");
            Console.WriteLine("\t <frame size> - The size of frames to create.");
        }
    }
}