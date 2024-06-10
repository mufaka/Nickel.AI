using Nickel.AI.Tokenization;

namespace Nickel.AI.TextTokenizer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var tokenizer = new TiktokenTokenizer();
            var toEncode = "Hello, World!";

            var encoded = tokenizer.Encode(toEncode);

            Console.WriteLine(String.Join(',', encoded));

            var decoded = tokenizer.Decode(encoded);

            Console.WriteLine(decoded);
        }
    }
}
