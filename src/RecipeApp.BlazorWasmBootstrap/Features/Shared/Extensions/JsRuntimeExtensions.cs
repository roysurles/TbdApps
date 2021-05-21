using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.JSInterop;

using RecipeApp.BlazorWasmBootstrap.Features.Shared.Models;

namespace RecipeApp.BlazorWasmBootstrap.Features.Shared.Extensions
{
    public static class JsRuntimeExtensions
    {
        public static ValueTask AlertAsync(this IJSRuntime jsRuntime, string text) =>
            jsRuntime.InvokeVoidAsync("alert", text);

        public static ValueTask<bool> ConfirmAsync(this IJSRuntime jsRuntime, string text) =>
            jsRuntime.InvokeAsync<bool>("confirm", text);

        public static ValueTask SetFocus(this IJSRuntime jsRuntime, string elementId) =>
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
    }
}
