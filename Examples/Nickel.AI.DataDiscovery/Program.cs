using Microsoft.Data.Analysis;
using Nickel.AI.Data;
using Spectre.Console;

namespace Nickel.AI.DataDiscovery
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // TODO: command line args for data source path, storage location, etc.
            try
            {
                // define a chunk size to work with. spotify data is only a few thousand rows
                const int chunkSize = 200;

                // platform independent paths
                var sourcePath = Path.Combine("Data", "Most Streamed Spotify Songs 2024.csv");
                var storagePath = Path.Combine("Storage", "Spotify");

                var loader = new CsvDataLoader(sourcePath, chunkSize, true);
                var storage = new CsvDataFrameStorage(storagePath);
                ChunkedData chunkedData = new ChunkedData();

                // initialize chunks if needed. just assume not initialized if storage path doesn't exist.
                if (!Directory.Exists(storagePath))
                {
                    Directory.CreateDirectory(storagePath);
                    chunkedData.Initialize(loader, storage);
                }
                else
                {
                    chunkedData.Load(storage);
                }

                // TODO: It would be nice to know the shape of the data (rows, cols).
                ShowTop(chunkedData, 250);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void ShowTop(ChunkedData chunkedData, int rows)
        {
            // use Spectre.Console table to display
            var table = new Table();

            var frameEnumerator = chunkedData.GetFrames();
            int displayCounter = 0;

            // enumerate frames because row count may be bigger than chunk
            while (frameEnumerator.MoveNext())
            {
                if (displayCounter >= rows) break;

                // first record, define table columns
                if (displayCounter == 0)
                {
                    var frameColumns = frameEnumerator.Current.Data!.Columns;

                    foreach (DataFrameColumn column in frameColumns)
                    {
                        table.AddColumn($"[green]{column.Name}[/]");
                    }
                }

                foreach (DataFrameRow dataFrameRow in frameEnumerator.Current.Data!.Rows)
                {
                    // Spectre.Console treats [ and ] as markup, need to sanitize
#pragma warning disable CS8602 // Dereference of a possibly null reference. x will not be null. x.Value is checked for null but IDE still warns.
                    var rowData = dataFrameRow.GetValues().Select(
                        x => x.Value != null
                        ? x.Value.ToString().Replace('[', ' ').Replace(']', ' ')
                        : "").ToArray();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    table.AddRow(rowData);
                    displayCounter++;

                    if (displayCounter >= rows) break;
                }
            }

            AnsiConsole.Write(table);
        }
    }
}
