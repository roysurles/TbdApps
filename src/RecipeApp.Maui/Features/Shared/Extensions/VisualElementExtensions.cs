namespace RecipeApp.Maui.Features.Shared.Extensions;

public static class VisualElementExtensions
{

    public static Task DisplayOkAlertAsync(this object anything, string title, string message) =>
        App.Current.MainPage.DisplayAlert(title, message, Constants.AlertButtonText.OK);

    public static Task DisplayCancelAlertAsync(this object anything, string title, string message) =>
        App.Current.MainPage.DisplayAlert(title, message, Constants.AlertButtonText.Cancel);

    public static Task<bool> DisplayOkCancelAlertAsync(this object anything, string title, string message) =>
        App.Current.MainPage.DisplayAlert(title, message, Constants.AlertButtonText.OK, Constants.AlertButtonText.Cancel);

    public static Task<bool> DisplayYesNoAlertAsync(this object anything, string title, string message) =>
        App.Current.MainPage.DisplayAlert(title, message, Constants.AlertButtonText.Yes, Constants.AlertButtonText.No);

    public static Task DisplaySnackbarAsync(this object anything, string message
        , Action action = null, string actionButtonText = "OK", TimeSpan? duration = null
        , SnackbarOptions snackbarOptions = null, CancellationToken cancellationToken = default) =>
        App.Current.MainPage.DisplaySnackbar(message, action, actionButtonText, duration, snackbarOptions, cancellationToken);

    /// <summary>
    /// Create new IToast
    /// NOTE:  This is IDisposable, make sure to dispose!
    /// </summary>
    /// <param name="message">Toast message</param>
    /// <param name="toastDuration">Toast duration</param>
    /// <param name="textSize">Toast text size</param>
    /// <returns>New instance of Toast</returns>
    public static IToast MakeToast(this object anything, string message, ToastDuration toastDuration = ToastDuration.Short, double textSize = 14) =>
        Toast.Make(message, toastDuration, textSize);

    public static async Task<IToast> ShowToastAsync(this object anything, string message, ToastDuration toastDuration = ToastDuration.Short, double textSize = 14, CancellationToken cancellationToken = default)
    {
        var toast = Toast.Make(message, toastDuration, textSize);
        await toast.Show(cancellationToken);
        return toast;
    }

    public static async Task<IToast> ShowSnackbarAndToastAsync(this object anything, string message
        , Action action = null, string actionButtonText = "OK", TimeSpan? duration = null
        , SnackbarOptions snackbarOptions = null, ToastDuration toastDuration = ToastDuration.Short
        , double textSize = 14, CancellationToken cancellationToken = default)
    {
        var snackBarTask = DisplaySnackbarAsync(anything, message, action, actionButtonText, duration, snackbarOptions, cancellationToken);
        var toastTask = ShowToastAsync(anything, message, toastDuration, textSize, cancellationToken);
        await Task.WhenAll(snackBarTask, toastTask);

        return await toastTask;
    }
}
