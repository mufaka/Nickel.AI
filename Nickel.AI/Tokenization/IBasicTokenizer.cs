namespace Nickel.AI.Tokenization
{
    public interface IBasicTokenizer
    {
        public int[] Encode(string text);
        public string? Decode(int[] tokens);
    }
}
