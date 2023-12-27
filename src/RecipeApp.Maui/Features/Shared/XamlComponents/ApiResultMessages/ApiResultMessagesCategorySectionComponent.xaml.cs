namespace RecipeApp.Maui.Features.Shared.XamlComponents.ApiResultMessages;

public partial class ApiResultMessagesCategorySectionComponent : ContentView
{
    public ApiResultMessagesCategorySectionComponent()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty ApiResultMessagesProperty =
        BindableProperty.Create(nameof(ApiResultMessages), typeof(IList<IApiResultMessageModel>), typeof(ApiResultMessagesCategorySectionComponent), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (ApiResultMessagesCategorySectionComponent)bindable;

        control.IsVisible = control.ApiResultMessages.Any();
    });

    public IList<IApiResultMessageModel> ApiResultMessages
    {
        get => (IList<IApiResultMessageModel>)GetValue(ApiResultMessagesProperty);
        set => SetValue(ApiResultMessagesProperty, value);
    }

    public static readonly BindableProperty CaptionProperty =
        BindableProperty.Create(nameof(Caption), typeof(string), typeof(ApiResultMessagesCategorySectionComponent));
    public string Caption
    {
        get => (string)GetValue(CaptionProperty);
        set => SetValue(CaptionProperty, value);
    }

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ApiResultMessagesCategorySectionComponent));
    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        // TODO:  copy to clipboard
        // https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/data/clipboard?view=net-maui-8.0
        await Clipboard.Default.SetTextAsync("This text was highlighted in the UI.");
    }
}