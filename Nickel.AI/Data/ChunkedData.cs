using Microsoft.Data.Analysis;

namespace Nickel.AI.Data
{
    public class ChunkedData
    {
        private List<ChunkedDataFrame> _frames = new List<ChunkedDataFrame>();
        public ChunkedDataMeta MetaData { get; private set; } = new ChunkedDataMeta();

        public List<ChunkedDataFrame> Frames { get { return _frames; } }

        public void Initialize(IDataLoader dataLoader, IDataFrameStorage storage)
        {
            int ordinal = 1;

            // NOTE: IDataFrameStorage implementations should handle existing data? Clear? Warn? Abort?

            foreach (var frame in dataLoader.LoadData(storage.FrameSize))
            {
                var chunkedData = new ChunkedDataFrame(storage, ordinal);
                chunkedData.Data = frame;

                MetaData.RowCount += frame.Rows.Count;
                MetaData.ColumnCount = frame.Columns.Count;
                chunkedData.Save();
                chunkedData.Unload();

                _frames.Add(chunkedData);

                ordinal++;
            }

            storage.SaveMetaData(MetaData);
        }

        public void Load(IDataFrameStorage storage)
        {
            var metaData = storage.LoadMetaData();
            if (metaData != null) { MetaData = metaData; }
            _frames = storage.LoadChunks();
        }

        public IEnumerator<ChunkedDataFrame> GetFrames()
        {
            foreach (var frame in _frames)
            {
                yield return frame;
            }
        }

        public IEnumerator<DataFrameRow> GetRows()
        {
            foreach (var frame in _frames)
            {
                foreach (DataFrameRow row in frame.Data!.Rows)
                {
                    yield return row;
                }
            }
        }

        // TODO: Implement IDataView for this? 

        // TODO: Implement DataFrame methods that operate on all chunks. Map/Reduce?
    }
}
