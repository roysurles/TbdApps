namespace RecipeApp.Maui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // https://www.youtube.com/watch?v=ddmZ6k1GIkM
        Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
    }
}
