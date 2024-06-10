using Nickel.AI.Chunking;
using Nickel.AI.Tokenization;
using System.Text;

namespace Nickel.AI.Extraction
{
    public class TextExtractor : ITextExtractor
    {
        private IChunker _chunker;
        private IBasicTokenizer _tokenizer;

        public TextExtractor(IChunker chunker, IBasicTokenizer tokenizer)
        {
            _chunker = chunker;
            _tokenizer = tokenizer;
        }

        public ExtractedDocument Extract(Uri uri)
        {
            switch (uri.Scheme)
            {
                case "http":
                case "https":
                    return ChunkExtractedDocument(new UrlTextExtractor().Extract(uri));
                case "file":
                    return ChunkExtractedDocument(new FileTextExtractor().Extract(uri));

            }

            throw new ArgumentException($"Unable to extract text for a Uri with scheme {uri.Scheme}.", "uri");
        }

        private ExtractedDocument ChunkExtractedDocument(ExtractedDocument extractedDocument)
        {
            if (_chunker == null)
            {
                return extractedDocument;
            }

            var chunkedDocument = new ExtractedDocument();
            chunkedDocument.Header = extractedDocument.Header;
            chunkedDocument.Footer = extractedDocument.Footer;

            var buff = new StringBuilder();

            foreach (string paragraph in extractedDocument.Paragraphs)
            {
                if (!String.IsNullOrWhiteSpace(paragraph))
                {
                    buff.AppendLine(paragraph);
                }
            }

            var chunks = _chunker.GetChunks(_tokenizer, buff.ToString());

            foreach (string chunk in chunks)
            {
                chunkedDocument.Paragraphs.Add(chunk);
            }

            return chunkedDocument;
        }
    }
}
