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
    }
}
