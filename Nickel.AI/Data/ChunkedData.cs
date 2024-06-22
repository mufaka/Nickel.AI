using Microsoft.Data.Analysis;
using Microsoft.ML;

namespace Nickel.AI.Data
{
    public class ChunkedData
    {
        private List<ChunkedDataFrame> _frames = new List<ChunkedDataFrame>();
        private long? _rowCount = 0;
        public List<ChunkedDataFrame> Frames { get { return _frames; } }

        public void Initialize(IDataLoader dataLoader, IDataFrameStorage storage)
        {
            int ordinal = 1;

            // NOTE: IDataFrameStorage implementations should handle existing data? Clear? Warn? Abort?

            foreach (var frame in dataLoader.LoadData())
            {
                var chunkedData = new ChunkedDataFrame(storage, ordinal);
                chunkedData.Data = frame;
                _rowCount += ((IDataView)frame).GetRowCount();
                chunkedData.Save();
                chunkedData.Unload();

                _frames.Add(chunkedData);

                ordinal++;
            }
        }

        public void Load(IDataFrameStorage storage)
        {
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
