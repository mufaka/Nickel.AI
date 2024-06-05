using Microsoft.Data.Analysis;

namespace Nickel.AI.Data
{
    public class ChunkedDataFrame
    {
        private IDataFrameStorage _dataFrameStorage;
        private int _ordinal;
        private DataFrame? _data;

        public ChunkedDataFrame(IDataFrameStorage dataFrameStorage, int ordinal)
        {
            _dataFrameStorage = dataFrameStorage;
            _ordinal = ordinal;
        }

        public DataFrame? Data
        {
            get
            {
                if (_data == null)
                {
                    _data = _dataFrameStorage.Load(_ordinal);
                }
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public void Save()
        {
            if (_data != null)
            {
                _dataFrameStorage.Save(_ordinal, _data);
            }
        }

        public void Unload()
        {
            _data = null;
        }

        public string FileName
        {
            get
            {
                return _dataFrameStorage.GetFileName(_ordinal);
            }
        }
    }
}
