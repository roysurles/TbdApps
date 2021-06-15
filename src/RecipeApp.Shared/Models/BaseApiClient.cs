using System.Net.Http;

namespace RecipeApp.Shared.Models
{
    public abstract class BaseApiClient
    {
        public BaseApiClient(HttpClient httpClient) =>
            HttpClient = httpClient;

        protected HttpClient HttpClient { get; }
    }
}
