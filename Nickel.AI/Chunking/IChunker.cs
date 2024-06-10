using Nickel.AI.Tokenization;

namespace Nickel.AI.Chunking
{
    public interface IChunker
    {
        // NOTE: Leave chunking logic / parameters to implementations.
        string[] GetChunks(IBasicTokenizer tokenizer, string text);
    }
}
