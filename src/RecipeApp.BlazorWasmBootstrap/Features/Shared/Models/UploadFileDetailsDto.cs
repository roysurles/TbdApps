namespace RecipeApp.BlazorWasmBootstrap.Features.Shared.Models
{
    public class UploadFileDetailsDto
    {
        public string Name { get; set; } =
            string.Empty;

        public byte[] Data { get; set; }
    }
}
