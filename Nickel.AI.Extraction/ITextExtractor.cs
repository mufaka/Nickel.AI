namespace Nickel.AI.Extraction
{
    public interface ITextExtractor
    {
        ExtractedDocument Extract(Uri uri);
    }
}
