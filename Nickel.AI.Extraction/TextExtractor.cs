namespace Nickel.AI.Extraction
{
    public class TextExtractor : ITextExtractor
    {
        public ExtractedDocument Extract(Uri uri)
        {
            switch (uri.Scheme)
            {
                case "http":
                case "https":
                    return new UrlTextExtractor().Extract(uri);
                case "file":
                    return new FileTextExtractor().Extract(uri);

            }

            throw new ArgumentException($"Unable to extract text for a Uri with scheme {uri.Scheme}.", "uri");
        }
    }
}
