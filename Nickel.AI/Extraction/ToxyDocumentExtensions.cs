using Toxy;

namespace Nickel.AI.Extraction
{
    public static class ToxyDocumentExtensions
    {
        public static ExtractedDocument ToExtractedDocument(this ToxyDocument document)
        {
            var extractedDocument = new ExtractedDocument
            {
                Header = document.Header,
                Footer = document.Footer
            };

            foreach (ToxyParagraph paragraph in document.Paragraphs)
            {
                extractedDocument.Paragraphs.Add(paragraph.Text);
            }

            return extractedDocument;
        }
    }
}
