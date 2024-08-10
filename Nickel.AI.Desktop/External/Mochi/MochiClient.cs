using RestSharp;
using RestSharp.Authenticators;

namespace Nickel.AI.Desktop.External.Mochi
{
    public class MochiClient
    {
        private RestClient _restClient;

        public MochiClient(string apiKey)
        {
            var options = new RestClientOptions("https://app.mochi.cards/api")
            {
                Authenticator = new HttpBasicAuthenticator(apiKey, "")
            };
            _restClient = new RestClient(options);
        }

        public async Task<MochiDeckListResponse> GetDeckList()
        {
            var request = new RestRequest("/decks/");
            return await _restClient.GetAsync<MochiDeckListResponse>(request);
        }

        public async Task<MochiDeck> CreateDeck(MochiDeck newDeck)
        {
            var request = new RestRequest("/decks/");
            // NOTE: Because property name has hyphen, have to use dictionary.
            //       Can't just serialize object because it has "extra" properties
            //       like "id" that aren't expected for CreateDeck
            request.AddBody(new Dictionary<string, string>()
            {
                { "name", newDeck.Name },
                { "parent-id", newDeck.ParentId }
            });

            return await _restClient.PostAsync<MochiDeck>(request);
        }

        public async Task<MochiCard> CreateCard(MochiCard newCard)
        {
            var request = new RestRequest("/cards/");
            request.AddBody(newCard);

            return await _restClient.PostAsync<MochiCard>(request);
        }
    }
}
