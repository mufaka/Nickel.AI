#pragma warning disable SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
using Microsoft.SemanticKernel.Text;
using Nickel.AI.Tokenization;

namespace Nickel.AI.Chunking
{
    public class SemanticKernelTextChunker : IChunker
    {
        private int _targetTokenCount = 0;

        public SemanticKernelTextChunker(int targetTokenCount)
        {
            _targetTokenCount = targetTokenCount;
        }

        public string[] GetChunks(IBasicTokenizer tokenizer, string text)
        {
            int TokenCounter(string text)
            {
                return tokenizer.Encode(text).Length;
            }

            return [.. TextChunker.SplitPlainTextLines(text, _targetTokenCount, TokenCounter)];
        }
    }
}