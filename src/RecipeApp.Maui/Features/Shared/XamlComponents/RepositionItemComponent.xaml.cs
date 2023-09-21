namespace RecipeApp.Maui.Features.Shared.XamlComponents;

public partial class RepositionItemComponent : ContentView
{
    public RepositionItemComponent()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(RepositionItemComponent));
    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly BindableProperty MoveFirstCommandProperty =
        BindableProperty.Create(nameof(MoveFirstCommand), typeof(ICommand), typeof(RepositionItemComponent));
    public ICommand MoveFirstCommand
    {
        get => (ICommand)GetValue(MoveFirstCommandProperty);
        set => SetValue(MoveFirstCommandProperty, value);
    }

    private void MoveFirstImageButton_Clicked(object sender, EventArgs e)
    {
        MoveFirstCommand?.Execute(CommandParameter);
    }

    //public object CommandParameter { get; set; }
}