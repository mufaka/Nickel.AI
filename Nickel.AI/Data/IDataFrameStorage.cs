﻿using Microsoft.Data.Analysis;

namespace Nickel.AI.Data
{
    public interface IDataFrameStorage
    {
        string RootPath { get; set; }
        DataFrame Load(int ordinal);
        void Save(int ordinal, DataFrame data);
        List<ChunkedDataFrame> LoadChunks();
        string GetFileName(int ordinal);
    }
}
