using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RecipeApp.Shared.Models
{
    public abstract class BaseApiClient
    {
        protected BaseApiClient(HttpClient httpClient, string controllerPath)
        {
            HttpClient = httpClient;
            ControllerPath = controllerPath;
        }

        protected HttpClient HttpClient { get; }

        protected string ControllerPath { get; }

        [SuppressMessage("Design", "RCS1090:Add call to 'ConfigureAwait' (or vice versa).", Justification = "<Pending>")]
        protected async Task<TResult> PostAsJsonExAsync<TResult, TRequest>(string requestUri
            , TRequest value
            , JsonSerializerOptions jsonSerializerOptions = null
            , JsonSerializerOptions jsonDeSerializerOptions = null
            , CancellationToken cancellationToken = default)
        {
            using var response = await HttpClient.PostAsJsonAsync(requestUri
                , value
                , jsonSerializerOptions
                , cancellationToken);

            return JsonSerializer.Deserialize<TResult>(
                await response.Content.ReadAsStringAsync(cancellationToken)
                , jsonDeSerializerOptions);
        }

        [SuppressMessage("Design", "RCS1090:Add call to 'ConfigureAwait' (or vice versa).", Justification = "<Pending>")]
        protected async Task<TResult> PutAsJsonExAsync<TResult, TRequest>(string requestUri
            , TRequest value
            , JsonSerializerOptions jsonSerializerOptions = null
            , JsonSerializerOptions jsonDeSerializerOptions = null
            , CancellationToken cancellationToken = default)
        {
            using var response = await HttpClient.PutAsJsonAsync(requestUri
                , value
                , jsonSerializerOptions
                , cancellationToken);

            return JsonSerializer.Deserialize<TResult>(
                await response.Content.ReadAsStringAsync(cancellationToken)
                , jsonDeSerializerOptions);
        }

        [SuppressMessage("Design", "RCS1090:Add call to 'ConfigureAwait' (or vice versa).", Justification = "<Pending>")]
        protected async Task<TResult> PatchAsJsonExAsync<TResult, TRequest>(string requestUri
            , TRequest value
            , JsonSerializerOptions jsonSerializerOptions = null
            , JsonSerializerOptions jsonDeSerializerOptions = null
            , CancellationToken cancellationToken = default)
        {
            using var content = new StringContent(JsonSerializer.Serialize(value, jsonSerializerOptions));
            using var response = await HttpClient.PatchAsync(requestUri, content, cancellationToken);

            return JsonSerializer.Deserialize<TResult>(
                await response.Content.ReadAsStringAsync(cancellationToken)
                , jsonDeSerializerOptions);
        }

        [SuppressMessage("Design", "RCS1090:Add call to 'ConfigureAwait' (or vice versa).", Justification = "<Pending>")]
        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        protected async Task<TResult> DeleteAsJsonExAsync<TResult, TRequest>(string requestUri
            , TRequest value
            , JsonSerializerOptions jsonSerializerOptions = null
            , JsonSerializerOptions jsonDeSerializerOptions = null
            , CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            if (value is not null)
            {
                using var content = new StringContent(JsonSerializer.Serialize(value, jsonSerializerOptions));
                request.Content = content;
            }

            using var response = await HttpClient.SendAsync(request, cancellationToken);

            return JsonSerializer.Deserialize<TResult>(
                await response.Content.ReadAsStringAsync(cancellationToken)
                , jsonDeSerializerOptions);
        }
    }
}
