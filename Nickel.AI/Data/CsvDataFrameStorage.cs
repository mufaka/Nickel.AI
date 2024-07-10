using Microsoft.Data.Analysis;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Nickel.AI.Data
{
    public class CsvDataFrameStorage : IDataFrameStorage
    {
        public string RootPath { get; set; }
        public int FrameSize { get; set; } = 1000;

        public CsvDataFrameStorage(string rootPath)
        {
            RootPath = rootPath;
        }

        public string GetFileName(int ordinal)
        {
            return Path.Combine(RootPath, $"{ordinal}.csv");
        }

        public DataFrame Load(int ordinal)
        {
            var fileName = GetFileName(ordinal);
            using (var stream = File.OpenRead(fileName))
            {
                // NOTE: guessRows defaults to 10 and will attempt to determine a datatype. 
                //       Using FrameSize as this is the only way to avoid type conversion
                //       errors completely. This should be OK because we should be chunking
                //       the data into reasonable sized chunks that shouldn't take too long
                //       to inspect.
                return DataFrame.LoadCsv(stream, guessRows: FrameSize);
            }
        }

        public void Save(int ordinal, DataFrame data)
        {
            var fileName = GetFileName(ordinal);

            // NOTE: File.OpenWrite will append, need to overwrite existing data
            //       in case chunk size has changed for an existing file.
            using (var stream = File.Open(fileName, FileMode.Create))
            {
                DataFrame.SaveCsv(data, stream);
            }
        }

        public List<ChunkedDataFrame> LoadChunks()
        {
            var list = new List<ChunkedDataFrame>();

            // order the files numerically
            var files = Directory.EnumerateFiles(RootPath, "*.csv", SearchOption.TopDirectoryOnly)
                .OrderBy(file => Regex.Replace(Path.GetFileNameWithoutExtension(file), @"\d+", match => match.Value.PadLeft(10, '0')));

            foreach (var file in files)
            {
                int ordinal;

                if (Int32.TryParse(Path.GetFileNameWithoutExtension(file), out ordinal))
                {
                    list.Add(new ChunkedDataFrame(this, ordinal));
                }
            }

            return list;
        }

        private const string META_FILE_NAME = "chunked_meta.json";

        public void SaveMetaData(ChunkedDataMeta meta)
        {
            var serialized = JsonSerializer.Serialize(meta);
            File.WriteAllText(Path.Combine(RootPath, META_FILE_NAME), serialized);
        }

        public ChunkedDataMeta? LoadMetaData()
        {
            var metaFileName = Path.Combine(RootPath, META_FILE_NAME);

            if (File.Exists(metaFileName))
            {
                return JsonSerializer.Deserialize<ChunkedDataMeta>(File.ReadAllText(metaFileName));
            }

            return null;
        }
    }
}
