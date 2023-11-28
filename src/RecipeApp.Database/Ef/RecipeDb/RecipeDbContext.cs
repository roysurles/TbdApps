namespace RecipeApp.Database.Ef.RecipeDb;

public class RecipeDbContext : DbContext
{
    public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options) { }

    public DbSet<IngredientModel> Ingredients { get; set; }

    public DbSet<InstructionModel> Instructions { get; set; }

    public DbSet<IntroductionModel> Introductions { get; set; }
}
