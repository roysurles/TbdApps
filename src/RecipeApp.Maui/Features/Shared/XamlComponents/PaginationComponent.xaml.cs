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

            const int maxDisplayButtons = 3;
            const int startIndex = 0;

            control.PageNumbers.Clear();
            for (int i = 1; i < control.PageCount + 1; i++)
                control.PageNumbers.Add(i.ToString());

            control.PageNumberButtons.Clear();
            control.PageNumberButtons.Add("<<");
            control.PageNumberButtons.Add("<");

            control.PageNumberButtons.AddRange(control.PageNumbers.Skip(startIndex).Take(maxDisplayButtons));

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

            control.DescriptionLabel.Text = control.Description = $"Page {control.PageNumber} of {control.PageCount}; Total Items: {control.TotalItemCount}";
        });
    public int PageNumber
    {
        get => (int)GetValue(PageNumberProperty);
        set => SetValue(PageNumberProperty, value);
    }

    public ObservableCollection<string> PageNumbers { get; protected set; } = new();
    public ObservableCollection<string> PageNumberButtons { get; protected set; } = new();

    public string Description { get; set; }


    public event EventHandler<PaginationPageNumberChangedEventArgs> PageNumberChangedEvent;

    private void OnPageNumberButtonClicked(object sender, EventArgs e)
    {
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
}