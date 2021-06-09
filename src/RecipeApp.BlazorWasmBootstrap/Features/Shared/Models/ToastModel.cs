
using System.Text.Json.Serialization;

using RecipeApp.BlazorWasmBootstrap.Features.Shared.Enums;

namespace RecipeApp.BlazorWasmBootstrap.Features.Shared.Models
{
    public class ToastModel
    {
        public ToastModel() { }

        public ToastModel(ToastType toastType, string title, string content, string subTitle = null, int delay = 3000)
        {
            ToastType = toastType;
            Title = title;
            Content = content;
            SubTitle = subTitle;
            Delay = delay;
        }

        [JsonIgnore]
        public ToastType ToastType { get; set; }

        [JsonPropertyName("type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Type => ToastType.ToString();

        [JsonPropertyName("title")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Title { get; set; }

        [JsonPropertyName("subtitle")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SubTitle { get; set; }

        [JsonPropertyName("content")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Content { get; set; }

        [JsonPropertyName("delay")]
        public int Delay { get; set; } = 3000;

        [JsonPropertyName("img")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ToastImageModel Image { get; set; } = null;
    }

    public class ToastImageModel
    {
        [JsonPropertyName("src")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Source { get; set; }

        [JsonPropertyName("class")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Class { get; set; }

        [JsonPropertyName("alt")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Alt { get; set; }
    }
}
