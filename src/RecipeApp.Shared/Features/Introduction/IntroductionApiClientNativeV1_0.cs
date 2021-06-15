
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using RecipeApp.Shared.Models;

using Tbd.Shared.ApiResult;

namespace RecipeApp.Shared.Features.Introduction
{
    public class IntroductionApiClientNativeV1_0 : BaseApiClient // TODO, IIntroductionApiClientV1_0
    {
        public IntroductionApiClientNativeV1_0(HttpClient httpClient) : base(httpClient) { }

        [SuppressMessage("Design", "RCS1090:Add call to 'ConfigureAwait' (or vice versa).", Justification = "<Pending>")]
        public async Task<ApiResultModel<IEnumerable<IntroductionSearchResultDto>>> SearchAsync(
            IntroductionSearchRequestDto introductionSearchRequestDto
            , JsonSerializerOptions jsonSerializerOptions = null
            , JsonSerializerOptions jsonDeSerializerOptions = null
            , CancellationToken cancellationToken = default)
        {
            using var response = await HttpClient.PostAsJsonAsync("/api/v1.0/Introduction/search"
                , introductionSearchRequestDto
                , jsonSerializerOptions
                , cancellationToken);

            return JsonSerializer.Deserialize<ApiResultModel<IEnumerable<IntroductionSearchResultDto>>>(
                await response.Content.ReadAsStringAsync(cancellationToken), jsonDeSerializerOptions);
        }

        public Task<ApiResultModel<IntroductionDto>> GetAsync(Guid id
            , JsonSerializerOptions jsonSerializerOptions = null
            , CancellationToken cancellationToken = default) =>
            HttpClient.GetFromJsonAsync<ApiResultModel<IntroductionDto>>($"/api/v1.0/Introduction/{id}", jsonSerializerOptions, cancellationToken);

        [SuppressMessage("Design", "RCS1090:Add call to 'ConfigureAwait' (or vice versa).", Justification = "<Pending>")]
        public async Task<ApiResultModel<IntroductionDto>> InsertAsync(IntroductionDto introductionDto
            , JsonSerializerOptions jsonSerializerOptions = null
            , JsonSerializerOptions jsonDeSerializerOptions = null
            , CancellationToken cancellationToken = default)
        {
            using var response = await HttpClient.PostAsJsonAsync("/api/v1.0/Introduction/{id}"
                , introductionDto
                , jsonSerializerOptions
                , cancellationToken);

            return JsonSerializer.Deserialize<ApiResultModel<IntroductionDto>>(
                await response.Content.ReadAsStringAsync(cancellationToken), jsonDeSerializerOptions);
        }

        [SuppressMessage("Design", "RCS1090:Add call to 'ConfigureAwait' (or vice versa).", Justification = "<Pending>")]
        public async Task<ApiResultModel<IntroductionDto>> UpdateAsync(IntroductionDto introductionDto
            , JsonSerializerOptions jsonSerializerOptions = null
            , JsonSerializerOptions jsonDeSerializerOptions = null
            , CancellationToken cancellationToken = default)
        {
            using var response = await HttpClient.PutAsJsonAsync("/api/v1.0/Introduction/{id}"
                , introductionDto
                , jsonSerializerOptions
                , cancellationToken);

            return JsonSerializer.Deserialize<ApiResultModel<IntroductionDto>>(
                await response.Content.ReadAsStringAsync(cancellationToken), jsonDeSerializerOptions);
        }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        [SuppressMessage("Design", "RCS1090:Add call to 'ConfigureAwait' (or vice versa).", Justification = "<Pending>")]
        public async Task<ApiResultModel<int>> DeleteAsync(Guid id
            , JsonSerializerOptions jsonDeSerializerOptions = null
            , CancellationToken cancellationToken = default)
        {
            using var response = await HttpClient.DeleteAsync($"/api/v1.0/Introduction/{id}", cancellationToken);

            return JsonSerializer.Deserialize<ApiResultModel<int>>(
                await response.Content.ReadAsStringAsync(cancellationToken), jsonDeSerializerOptions);
        }
    }
}
