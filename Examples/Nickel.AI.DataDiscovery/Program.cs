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

                //ShowColumns(chunkedData);

                // TODO: It would be nice to know the shape of the data (rows, cols).

                ShowTop(chunkedData, 40, [
                    "All Time Rank", "Artist", "Track", "Album Name", "Release Date",
                    "Track Score", "Spotify Streams", "Spotify Popularity"]);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void ShowColumns(ChunkedData chunkedData)
        {
            if (chunkedData.Frames.Count > 0)
            {
                var frameColumns = chunkedData.Frames[0].Data!.Columns;

                foreach (DataFrameColumn column in frameColumns)
                {
                    Console.WriteLine(column.Name);
                }

            }
        }

        private static void ShowTop(ChunkedData chunkedData, int rows, string[] columns)
        {
            // use Spectre.Console table to display
            var table = new Table();
            table.Border(TableBorder.MinimalHeavyHead);
            table.ShowRowSeparators();

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

                    //foreach (DataFrameColumn column in frameColumns)
                    foreach (string columnName in columns)
                    {
                        table.AddColumn($"[green]{columnName}[/]");
                    }
                }

                foreach (DataFrameRow dataFrameRow in frameEnumerator.Current.Data!.Rows)
                {
                    // Spectre.Console treats [ and ] as markup, need to sanitize
                    var rowValues = dataFrameRow.GetValues();

                    // TODO: sort the row data in the order of passed columns
                    var rowData = new string[columns.Length];

                    for (int i = 0; i < columns.Length; i++)
                    {
                        var rowValue = rowValues.Where(x => x.Key == columns[i]).FirstOrDefault().Value;
                        rowData[i] = rowValue == null ? "" : rowValue.ToString().Replace('[', ' ').Replace(']', ' ');
                    }

                    table.AddRow(rowData);
                    displayCounter++;

                    if (displayCounter >= rows) break;
                }
            }

            AnsiConsole.Write(table);
        }
    }
}
