using Nickel.AI.Tokenization;

namespace Nickel.AI.Chunking
{
    public class NaiveContextualChunker : IChunker
    {
        // NOTE: If zero, return all in one "chunk"
        private int _targetTokenCount = 0;
        private float _overlapFactor = 0.2f;

        /// <summary>
        /// Returns chunks of text from the given text based on the target token count. The chunks of
        /// text will overlap based on the the overlap factor.
        /// </summary>
        /// <param name="targetTokenCount">The amount of tokens to include in each chunk. This should be equal or less than the embedding vector size.</param>
        /// <param name="overlapFactor">The percent of tokens that should overlap</param>
        public NaiveContextualChunker(int targetTokenCount = 0, float overlapFactor = 0.2f)
        {
            _targetTokenCount = targetTokenCount;
            _overlapFactor = overlapFactor;
        }

        public string[] GetChunks(IBasicTokenizer tokenizer, string text)
        {
            // NOTE: this can stand to more efficiently handle strings
            var chunks = new List<string>();

            // tokenize the entire string
            var tokens = tokenizer.Encode(text);

            // no chunking needed?, return given text
            if (tokens == null || _targetTokenCount == 0 || tokens.Length <= _targetTokenCount)
            {
                chunks.Add(text);
            }
            else
            {
                // NOTE: this can be more efficient with better array handling
                var chunkedTokens = new List<int>(_targetTokenCount);
                var overlapDelta = (int)(_targetTokenCount * _overlapFactor);

                for (int x = 0; x < tokens.Length; x++)
                {
                    chunkedTokens.Add(tokens[x]);

                    if (chunkedTokens.Count >= _targetTokenCount)
                    {
                        var chunk = tokenizer.Decode(chunkedTokens.ToArray());

                        if (chunk != null)
                        {
                            chunks.Add(chunk);
                        }

                        chunkedTokens.Clear();

                        // back up by overlap factor
                        int newIndex = x - overlapDelta;

                        if (newIndex > 0)
                        {
                            x = newIndex;
                        }
                    }
                }

                // anything left in chunkedTokens?
                if (chunks.Count > 0)
                {
                    var chunk = tokenizer.Decode(chunkedTokens.ToArray());

                    if (chunk != null)
                    {
                        chunks.Add(chunk);
                    }
                }
            }

            return chunks.ToArray();
        }
    }
}
