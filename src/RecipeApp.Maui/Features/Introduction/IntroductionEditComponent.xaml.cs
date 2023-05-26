using System.Windows.Input;

namespace RecipeApp.Maui.Features.Introduction;

public partial class IntroductionEditComponent : ContentView
{
    public IntroductionEditComponent()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(IntroductionEditComponent), defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (IntroductionEditComponent)bindable;
        control.TitleEntry.Text = newValue as string;
    });
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

    public static readonly BindableProperty DeleteIntroductionCommandProperty =
        BindableProperty.Create(nameof(DeleteIntroductionCommand), typeof(ICommand), typeof(IntroductionEditComponent));

    public ICommand DeleteIntroductionCommand
    {
        get => (ICommand)GetValue(DeleteIntroductionCommandProperty);
        set => SetValue(DeleteIntroductionCommandProperty, value);
    }

    public event EventHandler DeleteIntroductionClicked;

    private void DeleteIntroductionButton_Clicked(object sender, EventArgs e)
    {
        DeleteIntroductionClicked?.Invoke(this, EventArgs.Empty);
    }
}