﻿namespace RecipeApp.Shared.Features.Introduction;

public class IntroductionApiClientNativeV1_0 : BaseApiClient, IIntroductionApiClientNativeV1_0
{
    public IntroductionApiClientNativeV1_0(HttpClient httpClient, string controllerPath
        , JsonSerializerOptions defaultJsonSerializerOptions = null
        , JsonSerializerOptions defaultJsonDeSerializerOptions = null)
        : base(httpClient, controllerPath, defaultJsonSerializerOptions, defaultJsonDeSerializerOptions) { }

    public Task<ApiResultModel<List<IntroductionSearchResultDto>>> SearchAsync(
        IntroductionSearchRequestDto introductionSearchRequestDto
        , JsonSerializerOptions jsonSerializerOptions = null
        , JsonSerializerOptions jsonDeSerializerOptions = null
        , CancellationToken cancellationToken = default)
    {
        return PostAsJsonExAsync<ApiResultModel<List<IntroductionSearchResultDto>>, IntroductionSearchRequestDto>(
            $"{ControllerPath}/search"
            , introductionSearchRequestDto
            , jsonSerializerOptions
            , jsonDeSerializerOptions
            , cancellationToken);
    }

    public Task<ApiResultModel<IntroductionDto>> GetAsync(Guid id
        , JsonSerializerOptions jsonSerializerOptions = null
        , CancellationToken cancellationToken = default)
    {
        return HttpClient.GetFromJsonAsync<ApiResultModel<IntroductionDto>>(
            $"{ControllerPath}/{id}"
            , jsonSerializerOptions
            , cancellationToken);
    }

    public Task<ApiResultModel<IntroductionDto>> InsertAsync(IntroductionDto introductionDto
        , JsonSerializerOptions jsonSerializerOptions = null
        , JsonSerializerOptions jsonDeSerializerOptions = null
        , CancellationToken cancellationToken = default)
    {
        return PostAsJsonExAsync<ApiResultModel<IntroductionDto>, IntroductionDto>(
            ControllerPath
            , introductionDto
            , jsonSerializerOptions
            , jsonDeSerializerOptions
            , cancellationToken);
    }

    public Task<ApiResultModel<IntroductionDto>> UpdateAsync(IntroductionDto introductionDto
        , JsonSerializerOptions jsonSerializerOptions = null
        , JsonSerializerOptions jsonDeSerializerOptions = null
        , CancellationToken cancellationToken = default)
    {
        return PutAsJsonExAsync<ApiResultModel<IntroductionDto>, IntroductionDto>(
            ControllerPath
            , introductionDto
            , jsonSerializerOptions
            , jsonDeSerializerOptions
            , cancellationToken);
    }

    public Task<ApiResultModel<int>> DeleteAsync(Guid id
        , JsonSerializerOptions jsonSerializerOptions = null
        , JsonSerializerOptions jsonDeSerializerOptions = null
        , CancellationToken cancellationToken = default)
    {
        return DeleteAsJsonExAsync<ApiResultModel<int>, object>($"{ControllerPath}/{id}"
            , null
            , jsonSerializerOptions
            , jsonDeSerializerOptions
            , cancellationToken);
    }
}

public interface IIntroductionApiClientNativeV1_0
{
    Task<ApiResultModel<List<IntroductionSearchResultDto>>> SearchAsync(
        IntroductionSearchRequestDto introductionSearchRequestDto
        , JsonSerializerOptions jsonSerializerOptions = null
        , JsonSerializerOptions jsonDeSerializerOptions = null
        , CancellationToken cancellationToken = default);

    Task<ApiResultModel<IntroductionDto>> GetAsync(Guid id
        , JsonSerializerOptions jsonSerializerOptions = null
        , CancellationToken cancellationToken = default);

    Task<ApiResultModel<IntroductionDto>> InsertAsync(IntroductionDto introductionDto
        , JsonSerializerOptions jsonSerializerOptions = null
        , JsonSerializerOptions jsonDeSerializerOptions = null
        , CancellationToken cancellationToken = default);

    Task<ApiResultModel<IntroductionDto>> UpdateAsync(IntroductionDto introductionDto
        , JsonSerializerOptions jsonSerializerOptions = null
        , JsonSerializerOptions jsonDeSerializerOptions = null
        , CancellationToken cancellationToken = default);

    Task<ApiResultModel<int>> DeleteAsync(Guid id
        , JsonSerializerOptions jsonSerializerOptions = null
        , JsonSerializerOptions jsonDeSerializerOptions = null
        , CancellationToken cancellationToken = default);
}
