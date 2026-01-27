using CommunityToolkit.Maui.Extensions;

namespace RecipeApp.Maui.Features.Shared.XamlComponents.ApiResultMessages;

public partial class ApiResultMessagesButtonComponent : ContentView
{
    public ApiResultMessagesButtonComponent()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty ApiResultMessagesProperty =
        BindableProperty.Create(nameof(ApiResultMessages), typeof(IList<IApiResultMessageModel>), typeof(ApiResultMessagesButtonComponent), propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (ApiResultMessagesButtonComponent)bindable;

            control.IsVisible = control.ApiResultMessages.Any();

            var categoryCount = 0;
            if (control.HasInformations) categoryCount++;
            if (control.HasWarnings) categoryCount++;
            if (control.HasErrors) categoryCount++;
            if (control.HasUnhandledExceptions) categoryCount++;

            var maximumWidthRequest = control.IsVisible
                ? (categoryCount * 10) + 10
                : 0;
            control.MaximumWidthRequest = maximumWidthRequest;

            control.InformationsImageButton.IsVisible = control.HasInformations;
            control.WarningsImageButton.IsVisible = control.HasWarnings;
            control.ErrorsImageButton.IsVisible = control.HasErrors;
            control.UnhandledExceptionsImageButton.IsVisible = control.HasUnhandledExceptions;

            if (control.IsVisible)
            {
                control._isBlinking = true;
                control.StartBlinkingAnimationAsync();
            }
        });
    public IList<IApiResultMessageModel> ApiResultMessages
    {
        get => (IList<IApiResultMessageModel>)GetValue(ApiResultMessagesProperty);
        set => SetValue(ApiResultMessagesProperty, value);
    }

    protected bool HasInformations => ApiResultMessages.Any(x => x.MessageType.Equals(ApiResultMessageModelTypeEnumeration.Information));

    protected bool HasWarnings => ApiResultMessages.Any(x => x.MessageType.Equals(ApiResultMessageModelTypeEnumeration.Warning));

    protected bool HasErrors => ApiResultMessages.Any(x => x.MessageType.Equals(ApiResultMessageModelTypeEnumeration.Error));

    protected bool HasUnhandledExceptions => ApiResultMessages.Any(x => x.MessageType.Equals(ApiResultMessageModelTypeEnumeration.UnhandledException));

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        //var popup = new Popup();
        //popup.CanBeDismissedByTappingOutsideOfPopup = false;
        //popup.Content = new StackLayout
        //{
        //    Children =
        //        {
        //            new Label { Text = "This is a Popup!" },
        //            new Button { Text = "Close Popup", Command = new Command(() => popup.Close()) }
        //        }
        //};

        //App.Current.Windows[0].Page.ShowPopup(popup);

        // https://github.com/jfversluis/MauiToolkitPopupSample/blob/main/MauiToolkitPopupSample/PopupPage.xaml
        var apiResultMessagesPopupComponent = new ApiResultMessagesPopupComponent();
        apiResultMessagesPopupComponent.ApiResultMessages = ApiResultMessages;
        apiResultMessagesPopupComponent.Content.BackgroundColor = Application.Current.RequestedTheme == AppTheme.Dark ? Colors.Black : Colors.White;
        apiResultMessagesPopupComponent.PopupBorder.Stroke = Brush.Cyan;
        //App.Current.Windows[0].Page.ShowPopup(apiResultMessagesPopupComponent);
        Application.Current.Windows[0].Page.ShowPopup(apiResultMessagesPopupComponent);
    }

    private bool _isBlinking;
    private async Task StartBlinkingAnimationAsync()
    {
        do
        {
            await ThisBorder.FadeTo(0, 500);
            await ThisBorder.FadeTo(1, 500);
        }
        while (_isBlinking);
    }
}