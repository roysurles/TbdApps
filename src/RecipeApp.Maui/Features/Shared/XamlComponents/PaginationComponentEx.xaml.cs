using System.Windows.Input;

namespace RecipeApp.Maui.Features.Shared.XamlComponents;

public partial class PaginationComponentEx : ContentView
{
    public PaginationComponentEx()
    {
        InitializeComponent();

        var b1 = App.Current.Resources.TryGetValue("Primary", out object selectedButtonBackgroundColor);
        if (b1)
            SelectedButtonBackgroundColor = (Color)selectedButtonBackgroundColor;

        var b2 = App.Current.Resources.TryGetValue("Secondary", out object nonSelectedButtonBackgroundColor);
        if (b2)
            NonSelectedButtonBackgroundColor = (Color)nonSelectedButtonBackgroundColor;
    }

    public ObservableCollection<PaginationComponentButtonProperties> ButtonsProperties { get; protected set; } = new();

    public static readonly BindableProperty TotalItemCountProperty =
    BindableProperty.Create(nameof(TotalItemCount), typeof(int), typeof(PaginationComponentEx), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (PaginationComponentEx)bindable;

        control.DescriptiveLabel.Text = $"Page {control.PageNumber} of {control.PageCount}; Total Items: {control.TotalItemCount}";
    });
    public int TotalItemCount
    {
        get => (int)GetValue(TotalItemCountProperty);
        set => SetValue(TotalItemCountProperty, value);
    }

    public static readonly BindableProperty PageCountProperty =
        BindableProperty.Create(nameof(PageCount), typeof(int), typeof(PaginationComponentEx), propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (PaginationComponentEx)bindable;

            control.ButtonsProperties.Clear();
            control.ButtonsProperties.Add(new PaginationComponentButtonProperties { Text = "<<", BackgroundColor = control.NonSelectedButtonBackgroundColor });
            control.ButtonsProperties.Add(new PaginationComponentButtonProperties { Text = "<", BackgroundColor = control.NonSelectedButtonBackgroundColor });

            var min = Math.Max(1, control.PageNumber);
            var max = Math.Min(control.PageCount + 1, control.MaxNumericButtons + 1);
            for (int i = min; i < max; i++)
                control.ButtonsProperties.Add(new PaginationComponentButtonProperties { Text = i.ToString(), BackgroundColor = i == control.PageNumber ? control.SelectedButtonBackgroundColor : control.NonSelectedButtonBackgroundColor });

            control.ButtonsProperties.Add(new PaginationComponentButtonProperties { Text = ">", BackgroundColor = control.NonSelectedButtonBackgroundColor });
            control.ButtonsProperties.Add(new PaginationComponentButtonProperties { Text = ">>", BackgroundColor = control.NonSelectedButtonBackgroundColor });

            control.DescriptiveLabel.Text = $"Page {control.PageNumber} of {control.PageCount}; Total Items: {control.TotalItemCount}";
        });
    public int PageCount
    {
        get => (int)GetValue(PageCountProperty);
        set => SetValue(PageCountProperty, value);
    }

    public static readonly BindableProperty PageNumberProperty =
        BindableProperty.Create(nameof(PageNumber), typeof(int), typeof(PaginationComponentEx), defaultValue: 1, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (oldValue == newValue)
                return;

            var control = (PaginationComponentEx)bindable;
            SetButtonsBackgroundColor(control);
            control.DescriptiveLabel.Text = $"Page {control.PageNumber} of {control.PageCount}; Total Items: {control.TotalItemCount}";
        });
    public int PageNumber
    {
        get => (int)GetValue(PageNumberProperty);
        set => SetValue(PageNumberProperty, value);
    }

    public static readonly BindableProperty MaxNumericButtonsProperty = BindableProperty.Create(nameof(MaxNumericButtons), typeof(int), typeof(PaginationComponentEx), defaultValue: 3);
    public int MaxNumericButtons
    {
        get => (int)GetValue(MaxNumericButtonsProperty);
        set => SetValue(MaxNumericButtonsProperty, value);
    }

    public static readonly BindableProperty SelectedButtonBackgroundColorProperty = BindableProperty.Create(nameof(SelectedButtonBackgroundColor), typeof(Color), typeof(PaginationComponentEx), propertyChanged: (bindable, oldValue, newValue) =>
    {
        if (oldValue == newValue)
            return;

        var control = (PaginationComponentEx)bindable;
        control.SelectedButtonBackgroundColor = (Color)newValue;
        SetButtonsBackgroundColor(control);
    });
    public Color SelectedButtonBackgroundColor
    {
        get => (Color)GetValue(SelectedButtonBackgroundColorProperty);
        set => SetValue(SelectedButtonBackgroundColorProperty, value);
    }

    public static readonly BindableProperty NonSelectedButtonBackgroundColorProperty = BindableProperty.Create(nameof(NonSelectedButtonBackgroundColor), typeof(Color), typeof(PaginationComponentEx), propertyChanged: (bindable, oldValue, newValue) =>
    {
        if (oldValue == newValue)
            return;

        var control = (PaginationComponentEx)bindable;
        control.NonSelectedButtonBackgroundColor = (Color)newValue;
        SetButtonsBackgroundColor(control);
    });
    public Color NonSelectedButtonBackgroundColor
    {
        get => (Color)GetValue(NonSelectedButtonBackgroundColorProperty);
        set => SetValue(NonSelectedButtonBackgroundColorProperty, value);
    }

    private static void SetButtonsBackgroundColor(PaginationComponentEx control)
    {
        foreach (var button in control.ButtonsProperties)
            button.BackgroundColor = string.Equals(button.Text, control.PageNumber.ToString()) ? control.SelectedButtonBackgroundColor : control.NonSelectedButtonBackgroundColor;
    }

    public event EventHandler<PaginationPageNumberChangedEventArgs> PageNumberChangedEvent;

    private void OnPageNumberButtonClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var buttonText = ((Button)sender).Text;
        //var newPageNumber = GetPageNumberFor(buttonText, PageNumber, PageCount);
        var newPageNumber = Convert.ToInt32(buttonText);
        if (newPageNumber == PageNumber)
            return;

        var paginationPageNumberChangedEventArgs = new PaginationPageNumberChangedEventArgs
        {
            PreviousPageNumber = PageNumber,
            PageNumber = newPageNumber //Convert.ToInt32(((Button)sender).Text)
        };
        PageNumber = paginationPageNumberChangedEventArgs.PageNumber;

        PageNumberChangedCommand?.Execute(paginationPageNumberChangedEventArgs);
        PageNumberChangedEvent?.Invoke(this, paginationPageNumberChangedEventArgs);
    }

    public static readonly BindableProperty PageNumberChangedCommandProperty =
        BindableProperty.Create(nameof(PageNumberChangedCommand), typeof(ICommand), typeof(PaginationComponent));
    public ICommand PageNumberChangedCommand
    {
        get => (ICommand)GetValue(PageNumberChangedCommandProperty);
        set => SetValue(PageNumberChangedCommandProperty, value);
    }
}