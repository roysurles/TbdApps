using System.Net.Http;

namespace RecipeApp.Shared.Models
{
    public abstract class BaseApiClient
    {
        protected BaseApiClient(HttpClient httpClient) =>
            HttpClient = httpClient;

        protected HttpClient HttpClient { get; }
    }
}
