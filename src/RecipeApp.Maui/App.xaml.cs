namespace RecipeApp.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // TODO:  get theme from OS to initialize with
        // https://www.telerik.com/blogs/handling-light-dark-mode-dotnet-maui
        AppTheme currentTheme = (AppTheme)Application.Current.RequestedTheme;
        UserAppTheme = currentTheme;

        // https://blog.ewers-peters.de/implement-dark-mode-in-net-maui
        // UserAppTheme = AppTheme.Unspecified;
        // UserAppTheme = AppTheme.Dark;

        MainPage = new AppShell();
    }
}
