using Microsoft.EntityFrameworkCore;

using RecipeApp.Database.Ef.RecipeDb.Models;

namespace RecipeApp.Database.Ef.RecipeDb
{
    public class RecipeDbContext : DbContext
    {
        public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options) { }

        public DbSet<IngredientModel> Ingredients { get; set; }
    }
}
