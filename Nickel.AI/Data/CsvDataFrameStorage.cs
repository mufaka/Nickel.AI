using Microsoft.Data.Analysis;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Nickel.AI.Data
{
    public class CsvDataFrameStorage : IDataFrameStorage
    {
        public string RootPath { get; set; }

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
                return DataFrame.LoadCsv(stream);
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
