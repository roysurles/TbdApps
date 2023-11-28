namespace RecipeApp.Database.Ef.RecipeDb.Models;

[Table("Introduction")]
public class IntroductionModel
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(50)]
    public string Title { get; set; }

    [MaxLength(255)]
    public string Comment { get; set; }

    [MaxLength(255)]
    public string CreatedById { get; set; }

    public DateTime? CreatedOnUtc { get; set; }

    [MaxLength(255)]
    public string UpdatedById { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }
}
