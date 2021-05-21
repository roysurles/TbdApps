
using Microsoft.AspNetCore.Components;

namespace RecipeApp.BlazorWasmBootstrap.Features.Shared.Extensions
{
    public static class HtmlExtensions
    {
        public static MarkupString ToMarkupString(this string str) =>
            new(str);

        public static string RenderDisabled(this bool b) =>
            b ? "disabled" : "";
    }
}
