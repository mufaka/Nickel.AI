using System.Text;
using System.Text.RegularExpressions;

namespace Nickel.AI.Desktop.Utilities
{
    public static class TextUtilities
    {
        public static string WordWrap(string text, float characterWidth, float windowWidth)
        {
            if (String.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            // leave a couple character margin for error in window width
            int charsPerLine = (int)Math.Floor(windowWidth / characterWidth) - 2;

            var lines = Regex.Split(text, "\r\n|\r|\n");
            var buff = new StringBuilder();

            foreach (string line in lines)
            {
                buff.Append(WrapLine(line, charsPerLine));
                buff.AppendLine();
            }

            return buff.ToString().Trim();
        }

        public static string WrapLine(string line, int maxChars)
        {
            // NOTE: This predetermines the amount of chunks the line
            //       needs to be split into. Knowing that, we use the
            //       known indices for the splits to check if we are
            //       splitting on a space. If it's not a space and the next
            //       character is a space, split there. If not, work backwards
            //       to find the first occurence of a space and split on that. 

            // How many chunks?
            var chunks = Math.Ceiling((double)line.Length / maxChars);

            // How far back have we gone to find a good split
            int offSet = 0;
            var buff = new StringBuilder();

            // loop through the amount of chunks
            for (int i = 0; i < chunks; i++)
            {
                var chunkStart = i * maxChars - offSet;
                var chunkEnd = Math.Min(chunkStart + maxChars - 1, line.Length - 1);
                var nextChunkBegin = chunkEnd + 1;

                if (line[chunkEnd] != ' ' && nextChunkBegin < line.Length)
                {
                    int space = line.LastIndexOf(' ', chunkEnd, maxChars);

                    if (space != -1)
                    {
                        offSet += chunkEnd - space;
                        chunkEnd = space;
                        // it's possible that we need more chunks because we've split
                        // more times than the original estimate.
                        chunks = chunks + (offSet % maxChars);
                    }
                }

                // because we have possibly added to the chunk count, we may overflow.
                // break out of the loop, we are done.
                if (chunkStart > line.Length - 1 || chunkEnd > line.Length - 1) break;

                // indexer doesn't like maths in syntax, indexer is exclusive so need to add 1 to end
                int exclusiveEnd = chunkEnd + 1;
                buff.AppendLine(line[chunkStart..exclusiveEnd]);
            }

            return buff.ToString();
        }
    }
}
