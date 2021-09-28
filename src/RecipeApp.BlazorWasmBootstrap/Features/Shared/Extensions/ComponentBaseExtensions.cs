
using Microsoft.AspNetCore.Components;

namespace RecipeApp.BlazorWasmBootstrap.Features.Shared.Extensions
{
    public static class ComponentBaseExtensions
    {
        public static string ComponentName(this ComponentBase componentBase) =>
            componentBase.GetType().Name;
    }
}
