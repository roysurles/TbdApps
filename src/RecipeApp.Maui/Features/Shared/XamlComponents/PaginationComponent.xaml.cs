using System.Windows.Input;

namespace RecipeApp.Maui.Features.Shared.XamlComponents;

public partial class PaginationComponent : ContentView
{
    public PaginationComponent()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty TotalItemCountProperty =
        BindableProperty.Create(nameof(TotalItemCount), typeof(int), typeof(PaginationComponent), propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (PaginationComponent)bindable;

            control.DescriptionLabel.Text = control.Description = $"Page {control.PageNumber} of {control.PageCount}; Total Items: {control.TotalItemCount}";
        });
    public int TotalItemCount
    {
        get => (int)GetValue(TotalItemCountProperty);
        set => SetValue(TotalItemCountProperty, value);
    }

    public static readonly BindableProperty PageCountProperty =
        BindableProperty.Create(nameof(PageCount), typeof(int), typeof(PaginationComponent), propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (PaginationComponent)bindable;

            control.PageNumbers.Clear();
            for (int i = 1; i < control.PageCount + 1; i++)
                control.PageNumbers.Add(i.ToString());

            control.PageNumberButtons.Clear();
            control.PageNumberButtons.Add("<<");
            control.PageNumberButtons.Add("<");
            control.PageNumberButtons.InsertRange(2, control.GetPageNumberButtonsFor(control.PageNumbers, control.PageNumber, control.PageCount, control.MaxDisplayPageNumberButtons));
            control.PageNumberButtons.Add(">");
            control.PageNumberButtons.Add(">>");

            control.DescriptionLabel.Text = control.Description = $"Page {control.PageNumber} of {control.PageCount}; Total Items: {control.TotalItemCount}";
        });
    public int PageCount
    {
        get => (int)GetValue(PageCountProperty);
        set => SetValue(PageCountProperty, value);
    }

    public static readonly BindableProperty PageNumberProperty =
        BindableProperty.Create(nameof(PageNumber), typeof(int), typeof(PaginationComponent), propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (PaginationComponent)bindable;

            if (control.PageCount > control.MaxDisplayPageNumberButtons)
            {
                control.PageNumberButtons.RemoveRange(2, control.MaxDisplayPageNumberButtons);
                control.PageNumberButtons.InsertRange(2, control.GetPageNumberButtonsFor(control.PageNumbers, control.PageNumber, control.PageCount, control.MaxDisplayPageNumberButtons));
            }

            control.DescriptionLabel.Text = control.Description = $"Page {control.PageNumber} of {control.PageCount}; Total Items: {control.TotalItemCount}";
        });
    public int PageNumber
    {
        get => (int)GetValue(PageNumberProperty);
        set => SetValue(PageNumberProperty, value);
    }

    //public static readonly BindableProperty PageNumberAsStringProperty = BindableProperty.Create(nameof(PageNumberAsString), typeof(string), typeof(PaginationComponent), "0", BindingMode.OneWay);
    //public string PageNumberAsString
    //{
    //    get => GetValue(PageNumberProperty)?.ToString() ?? "0";
    //}

    public static readonly BindableProperty SelectedPageNumberButtonProperty = BindableProperty.Create(nameof(SelectedPageNumberButton), typeof(Button), typeof(PaginationComponent));
    public Button SelectedPageNumberButton
    {
        get => (Button)GetValue(SelectedPageNumberButtonProperty);
        set => SetValue(SelectedPageNumberButtonProperty, value);
    }

    public static readonly BindableProperty MaxDisplayPageNumberButtonsProperty = BindableProperty.Create(nameof(MaxDisplayPageNumberButtons), typeof(int), typeof(PaginationComponent), 3);
    public int MaxDisplayPageNumberButtons
    {
        get => (int)GetValue(MaxDisplayPageNumberButtonsProperty);
        set => SetValue(MaxDisplayPageNumberButtonsProperty, value);
    }

    public ObservableCollection<string> PageNumbers { get; protected set; } = new();
    public ObservableCollection<string> PageNumberButtons { get; protected set; } = new();

    public string Description { get; set; }


    public event EventHandler<PaginationPageNumberChangedEventArgs> PageNumberChangedEvent;

    private void OnPageNumberButtonClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var buttonText = ((Button)sender).Text;
        var newPageNumber = GetPageNumberFor(buttonText, PageNumber, PageCount);
        if (newPageNumber == PageNumber)
            return;

        var paginationPageNumberChangedEventArgs = new PaginationPageNumberChangedEventArgs
        {
            PreviousPageNumber = PageNumber,
            PageNumber = newPageNumber //Convert.ToInt32(((Button)sender).Text)
        };
        PageNumber = paginationPageNumberChangedEventArgs.PageNumber;
        SelectedPageNumberButton = button;

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

    private int GetPageNumberFor(string newValue, int currentPageNumber, int maxPageNumber)
    {
        if (newValue.IsNumeric())
            return Convert.ToInt32(newValue);

        if (string.Equals("<<", newValue))
            return 1;

        if (string.Equals("<", newValue))
            return Math.Max(currentPageNumber - 1, 1);

        if (string.Equals(">>", newValue))
            return maxPageNumber;

        if (string.Equals(">", newValue))
            return Math.Min(currentPageNumber + 1, maxPageNumber);

        return 1;
    }

    private IEnumerable<string> GetPageNumberButtonsFor(IEnumerable<string> pageNumbers, int currentPageNumber, int maxPageNumber, int maxDisplayButtons)
    {
        var leftRightButtonCount = maxDisplayButtons / 2;

        var startIndex = Math.Max(currentPageNumber - leftRightButtonCount - 1, 0);
        startIndex = Math.Min(startIndex, maxPageNumber - maxDisplayButtons);

        var result = pageNumbers.Skip(startIndex).Take(maxDisplayButtons);

        return result;
    }
}