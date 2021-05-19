
using Microsoft.AspNetCore.Components;

namespace RecipeApp.BlazorWasmBootstrap.Features.Shared.Extensions
{
    public static class StringExtensions
    {
        public static MarkupString ToMarkupString(this string str) =>
            new(str);
    }
}
