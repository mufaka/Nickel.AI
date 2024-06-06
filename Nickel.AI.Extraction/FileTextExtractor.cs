using System.Text;
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
            var extension = Path.GetExtension(filePath);

            if (extension != null && extension.ToLower() == ".txt")
            {
                return ExtractFromPlainText(filePath);
            }

            var documentParser = ParserFactory.CreateDocument(new ParserContext(filePath));
            var document = documentParser.Parse();

            return document.ToExtractedDocument();
        }

        // use a naive approach for .txt files
        private ExtractedDocument ExtractFromPlainText(string filePath)
        {
            var document = new ExtractedDocument();
            var buffer = new StringBuilder();

            using StreamReader reader = new StreamReader(filePath);

            var line = reader.ReadLine();

            while (line != null)
            {
                if (line.Trim().Length == 0)
                {
                    if (buffer.Length > 0)
                    {
                        document.Paragraphs.Add(buffer.ToString());
                        buffer.Clear();
                    }
                }
                else
                {
                    buffer.AppendLine(line);
                }

                line = reader.ReadLine();
            }

            if (buffer.Length > 0)
            {
                document.Paragraphs.Add(buffer.ToString());
            }

            return document;
        }
    }
}
