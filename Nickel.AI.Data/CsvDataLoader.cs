using CsvHelper;
using Microsoft.Data.Analysis;
using System.Globalization;

namespace Nickel.AI.Data
{
    /// <summary>
    /// CsvDataLoader provides a memory efficient means to load data
    /// into <see href="https://learn.microsoft.com/en-us/dotnet/api/microsoft.data.analysis.dataframe?view=ml-dotnet-preview"/>Microsoft.Data.Analysis.DataFrames</see>.
    /// </summary>
    public class CsvDataLoader : IDataLoader
    {
        private string _csvFileName;
        private int _frameSize;
        private bool _hasHeader;

        /// <summary>
        /// Constructs a new instance of CsvDataLoader
        /// </summary>
        /// <param name="csvFileName">The full path to the csv file.</param>
        /// <param name="frameSize">The amount of rows to load into each DataFrame.</param>
        /// <param name="hasHeader">Whether or not the csv file has a header row.</param>
        public CsvDataLoader(string csvFileName, int frameSize, bool hasHeader = true)
        {
            _csvFileName = csvFileName;
            _frameSize = frameSize;
            _hasHeader = hasHeader;
        }

        /// <summary>
        /// LoadData streams data from the source csv file into an enumerable of DataFrame. 
        /// </summary>
        /// <returns>An enumerable of <see href="https://learn.microsoft.com/en-us/dotnet/api/microsoft.data.analysis.dataframe?view=ml-dotnet-preview"/>Microsoft.Data.Analysis.DataFrame</see></returns>
        public IEnumerable<DataFrame> LoadData()
        {
            using (var reader = new StreamReader(_csvFileName))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();

                if (_hasHeader)
                {
                    csv.ReadHeader();
                }

                int counter = 0;
                var frame = new DataFrame(GetColumns(csv));

                while (csv.Read())
                {
                    counter++;

                    var record = new string[csv.ColumnCount];

                    for (int i = 0; i < csv.ColumnCount; i++)
                    {
                        record[i] = csv.GetField<string>(i);
                    }

                    // inPlace: true appends the row in place without creating a new DataFrame
                    frame.Append(record, inPlace: true);

                    if (counter == _frameSize)
                    {
                        // clone is used to ensure the reference to frame is disconnected
                        // from the return
                        var ret = frame.Clone();

                        counter = 0;
                        frame = new DataFrame(GetColumns(csv));

                        yield return ret;

                    }
                }

                yield return frame;
            }
        }

        // NOTE: Columns are redefined for each DataFrame because they are internally associated with the
        //       the DataFrameRows. When instantiating a DataFrame with columns that already have DataFrameRows associated,
        //       the new DataFrame will include those rows.
        private DataFrameColumn[] GetColumns(CsvReader csv)
        {
            DataFrameColumn[] columns = new DataFrameColumn[csv.ColumnCount];

            for (int i = 0; i < csv.ColumnCount; i++)
            {
                if (_hasHeader)
                {
                    columns[i] = new StringDataFrameColumn(csv.HeaderRecord[i]);
                }
                else
                {
                    columns[i] = new StringDataFrameColumn($"Column{i}");
                }
            }

            return columns;
        }
    }
}
