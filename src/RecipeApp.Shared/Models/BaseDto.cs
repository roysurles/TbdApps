namespace RecipeApp.Shared.Models;

public abstract class BaseDto
{
    public Guid Id { get; set; } =
        Guid.Empty;

    public string CreatedById { get; set; }

    public DateTime? CreatedOnUtc { get; set; }

    public string UpdatedById { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }

    public bool IsNew =>
        Id == Guid.Empty;
}
