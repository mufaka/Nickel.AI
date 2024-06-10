using Microsoft.ML.Tokenizers;

namespace Nickel.AI.Tokenization
{
    public class TiktokenTokenizer : IBasicTokenizer
    {
        public int[] Encode(string text)
        {
            var tokenizer = Tokenizer.CreateTiktokenForModel("gpt-4");
            return tokenizer.EncodeToIds(text).ToArray();
        }

        public string? Decode(int[] tokens)
        {
            var tokenizer = Tokenizer.CreateTiktokenForModel("gpt-4");
            return tokenizer.Decode(tokens);
        }
    }
}
