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


    public static readonly BindableProperty MoveUpCommandProperty =
        BindableProperty.Create(nameof(MoveUpCommand), typeof(ICommand), typeof(RepositionItemComponent));
    public ICommand MoveUpCommand
    {
        get => (ICommand)GetValue(MoveUpCommandProperty);
        set => SetValue(MoveUpCommandProperty, value);
    }


    public static readonly BindableProperty MoveDownCommandProperty =
        BindableProperty.Create(nameof(MoveDownCommand), typeof(ICommand), typeof(RepositionItemComponent));
    public ICommand MoveDownCommand
    {
        get => (ICommand)GetValue(MoveDownCommandProperty);
        set => SetValue(MoveDownCommandProperty, value);
    }


    public static readonly BindableProperty MoveLastCommandProperty =
        BindableProperty.Create(nameof(MoveLastCommand), typeof(ICommand), typeof(RepositionItemComponent));
    public ICommand MoveLastCommand
    {
        get => (ICommand)GetValue(MoveLastCommandProperty);
        set => SetValue(MoveLastCommandProperty, value);
    }

    private void MoveFirstImageButton_Clicked(object sender, EventArgs e) =>
        MoveFirstCommand?.Execute(CommandParameter);

    private void MoveUpImageButton_Clicked(object sender, EventArgs e) =>
        MoveUpCommand?.Execute(CommandParameter);

    private void MoveDownImageButton_Clicked(object sender, EventArgs e) =>
        MoveDownCommand?.Execute(CommandParameter);

    private void MoveLastImageButton_Clicked(object sender, EventArgs e) =>
        MoveLastCommand?.Execute(CommandParameter);
}