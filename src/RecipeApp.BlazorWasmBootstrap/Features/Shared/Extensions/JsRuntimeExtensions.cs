namespace RecipeApp.BlazorWasmBootstrap.Features.Shared.Extensions;

public static class JsRuntimeExtensions
{
    public static ValueTask RemoveAttributeAsync(this IJSRuntime jsRuntime, string elementId, string attributeName) =>
        jsRuntime.InvokeVoidAsync("siteModule.removeAttribute", elementId, attributeName);

    public static ValueTask SetAttributeAsync(this IJSRuntime jsRuntime, string elementId, string attributeName, string attributeValue) =>
        jsRuntime.InvokeVoidAsync("siteModule.setAttribute", elementId, attributeName, attributeValue);

    public static ValueTask<string> GetHtmlElementAttributeAsync(this IJSRuntime jsRuntime, string name) =>
        jsRuntime.InvokeAsync<string>("document.documentElement.getAttribute", name);

    /// <summary>
    /// https://stackoverflow.com/questions/54404940/set-attributes-of-html-tag-using-pure-javascript
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static ValueTask SetHtmlElementAttributeAsync(this IJSRuntime jsRuntime, string name, string value) =>
        jsRuntime.InvokeVoidAsync("document.documentElement.setAttribute", name, value);

    public static ValueTask<bool> PrefersDarkMode(this IJSRuntime jsRuntime) =>
        jsRuntime.InvokeAsync<bool>("siteModule.prefersDarkMode");

    public static ValueTask AlertAsync(this IJSRuntime jsRuntime, string text) =>
        jsRuntime.InvokeVoidAsync("alert", text);

    public static ValueTask<bool> ConfirmAsync(this IJSRuntime jsRuntime, string text) =>
        jsRuntime.InvokeAsync<bool>("confirm", text);

    public static ValueTask SetFocusAsync(this IJSRuntime jsRuntime, string elementId) =>
        jsRuntime.InvokeVoidAsync("siteModule.setFocus", elementId);

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

    public static async ValueTask CopyElementToClipboard(this IJSRuntime jsRuntime, string element) =>
        jsRuntime.InvokeVoidAsync("siteModule.copyElementToClipboard", element);

    public static ValueTask SnackAsync(this IJSRuntime jsRuntime, ToastType toastType, string content, int delay = 3000) =>
        jsRuntime.InvokeVoidAsync("$.snack", toastType.ToString(), content, delay);

    public static ValueTask ToastAsync(this IJSRuntime jsRuntime, ToastModel toastModel) =>
        jsRuntime.InvokeVoidAsync("$.toast", toastModel);
}
