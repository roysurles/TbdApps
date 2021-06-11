namespace RecipeApp.Blazor.Shared.Models
{
    public class UploadFileDetailsDto
    {
        public string Name { get; set; } =
            string.Empty;

        public byte[] Data { get; set; }
    }
}
