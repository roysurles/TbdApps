using System.Windows.Input;

namespace RecipeApp.Maui.Features.Introduction;

public partial class IntroductionEditComponent : ContentView
{
    public IntroductionEditComponent()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(IntroductionEditComponent), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (IntroductionEditComponent)bindable;
        control.TitleEntry.Text = newValue as string;
    });
    //public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(IntroductionEditComponent));
    public string Title
    {
        get => GetValue(TitleProperty) as string;
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty CommentsProperty = BindableProperty.Create(nameof(Comments), typeof(string), typeof(IntroductionEditComponent), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (IntroductionEditComponent)bindable;
        control.CommentsEntry.Text = newValue as string;
    });
    public string Comments
    {
        get => GetValue(CommentsProperty) as string;
        set => SetValue(CommentsProperty, value);
    }

    public static readonly BindableProperty SaveIntroductionCommandProperty =
        BindableProperty.Create(nameof(SaveIntroductionCommand), typeof(ICommand), typeof(IntroductionEditComponent));

    public ICommand SaveIntroductionCommand
    {
        get => (ICommand)GetValue(SaveIntroductionCommandProperty);
        set => SetValue(SaveIntroductionCommandProperty, value);
    }

    public event EventHandler SaveIntroductionClicked;

    private void SaveIntroductionButton_Clicked(object sender, EventArgs e)
    {
        SaveIntroductionClicked?.Invoke(this, EventArgs.Empty);
    }
}