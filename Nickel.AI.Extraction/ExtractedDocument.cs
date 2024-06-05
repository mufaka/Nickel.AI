using System.Text;

namespace Nickel.AI.Extraction
{
    // NOTE: This somewhat mimicks ToxyDocument for the purpose of having a generic type to return. Currently
    //       this library is just a facade of Toxy but can be extended by parsing into this common
    //       return type.
    public class ExtractedDocument
    {
        public string Header { get; set; } = String.Empty;
        public string Footer { get; set; } = String.Empty;
        public List<string> Paragraphs { get; set; } = new List<string>();

        public override string ToString()
        {
            var buff = new StringBuilder();

            buff.AppendLine($"Header");
            buff.AppendLine(Header);
            buff.AppendLine();

            foreach (var paragraph in Paragraphs)
            {
                buff.AppendLine(paragraph);
                buff.AppendLine();
            }

            buff.AppendLine($"Footer");
            buff.AppendLine(Footer);

            return buff.ToString();
        }
    }
}
