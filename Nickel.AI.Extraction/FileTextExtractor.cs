using Toxy;

namespace Nickel.AI.Extraction
{
    public class FileTextExtractor : ITextExtractor
    {
        public ExtractedDocument Extract(Uri uri)
        {
            return Extract(uri.LocalPath);
        }

        private ExtractedDocument Extract(string filePath)
        {
            var documentParser = ParserFactory.CreateDocument(new ParserContext(filePath));
            var document = documentParser.Parse();

            return document.ToExtractedDocument();
        }

    }
}
