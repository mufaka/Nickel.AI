using Newtonsoft.Json;

namespace Nickel.AI.Desktop.Models
{
    public class FlashCards
    {
        [JsonProperty("cards")]
        public List<Card> Cards { get; set; } = new List<Card>();

        public static FlashCards FromLlmResponse(string response)
        {
            /*
                NOTE: This relies on a prompt in the following form:

                Respond in json with the following format:

                {
                    "cards": [
                        { "question": "", "answer": "" }  
                    ]
                }

                The response is expected to be in Markdown format and the first code block is expected to be json in the provided format.
            */

            try
            {
                // within the response find ``` ... ```
                var firstTicks = response.IndexOf("```");
                var nextTicks = response.IndexOf("```", firstTicks + 4);
                var codeBlock = response.Substring(firstTicks + 4, nextTicks - (firstTicks + 4));

                // within the code block, find { ... }
                var firstBracket = codeBlock.IndexOf('{');
                var lastBracket = codeBlock.LastIndexOf('}');

                var json = codeBlock.Substring(firstBracket, (lastBracket - firstBracket) + 1);
                var cards = JsonConvert.DeserializeObject<FlashCards>(json);

                return cards ?? new FlashCards();
            }
            catch
            {
                // TODO: handle this more gracefully
                return new FlashCards();
            }
        }
    }

    public class Card
    {
        [JsonProperty("question")]
        public string Question { get; set; } = String.Empty;

        [JsonProperty("answer")]
        public string Answer { get; set; } = String.Empty;

        [JsonProperty("detail")]
        public string Detail { get; set; } = String.Empty;

        public bool Know { get; set; } = false;
    }
}
