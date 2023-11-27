namespace RecipeApp.BlazorWasmMud.Features.Shared.Extensions;

public static class JsRuntimeExtensions
{
    public static ValueTask<string> GetHtmlAttribute(this IJSRuntime jsRuntime, string name) =>
        jsRuntime.InvokeAsync<string>("siteModule.getHtmlAttribute", name);

    /// <summary>
    /// https://stackoverflow.com/questions/54404940/set-attributes-of-html-tag-using-pure-javascript
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static ValueTask SetHtmlAttribute(this IJSRuntime jsRuntime, string name, string value) =>
        jsRuntime.InvokeVoidAsync("siteModule.setHtmlAttribute", name, value);

    public static ValueTask<bool> PrefersDarkMode(this IJSRuntime jsRuntime) =>
        jsRuntime.InvokeAsync<bool>("siteModule.prefersDarkMode");

    public static ValueTask AlertAsync(this IJSRuntime jsRuntime, string text) =>
        jsRuntime.InvokeVoidAsync("alert", text);

    public static ValueTask<bool> ConfirmAsync(this IJSRuntime jsRuntime, string text) =>
        jsRuntime.InvokeAsync<bool>("confirm", text);

    public static ValueTask SetFocusAsync(this IJSRuntime jsRuntime, string elementId) =>
        jsRuntime.InvokeVoidAsync("siteModule.setFocus", elementId);

    public static ValueTask CopyToClipboard(this IJSRuntime jsRuntime, string elementId) =>
        jsRuntime.InvokeVoidAsync("siteModule.copyToClipboard", elementId);

    public static async ValueTask DownloadFileAsync(this IJSRuntime jsRuntime, byte[] data, string mimeType, string fileName)
    {
        if (data is null)
            await jsRuntime.AlertAsync("The byte array provided for Exporting was Null.");
        else
            await jsRuntime.InvokeVoidAsync("siteModule.downloadFile", Convert.ToBase64String(data), mimeType, fileName);
    }

    public static async ValueTask<UploadFileDetailsDto> UploadFileAsync(this IJSRuntime jsRuntime, string inputId)
    {
        var fileDetails = new UploadFileDetailsDto();
        JsonElement fileAsJson = await jsRuntime.InvokeAsync<JsonElement>("getUploadedFile", inputId);
        var fileAsString = fileAsJson.ToString();

        if (!string.IsNullOrWhiteSpace(fileAsString))
        {
            Dictionary<string, string> uploadData = JsonSerializer.Deserialize<Dictionary<string, string>>(fileAsString);
            fileDetails.Name = uploadData["fileName"];
            fileDetails.Data = Convert.FromBase64String(uploadData["fileData"]);
        }
        return fileDetails;
    }
}
