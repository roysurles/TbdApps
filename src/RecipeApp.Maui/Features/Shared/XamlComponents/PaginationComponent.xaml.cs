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
                control.PageNumbers.Add(i);

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

    public ObservableCollection<int> PageNumbers { get; set; } = new();

    public string Description { get; set; }


    public event EventHandler<PaginationPageNumberChangedEventArgs> PageNumberChangedEvent;

    private void OnPageNumberButtonClicked(object sender, EventArgs e)
    {
        var previousPageNumber = PageNumber;
        PageNumber = Convert.ToInt32(((Button)sender).Text);
        PageNumberChangedEvent?.Invoke(this, new PaginationPageNumberChangedEventArgs { PageNumber = PageNumber, PreviousPageNumber = previousPageNumber });
    }

    public static readonly BindableProperty PageNumberChangedCommandProperty =
        BindableProperty.Create(nameof(PageNumberChangedCommand), typeof(int), typeof(PaginationComponent), propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (PaginationComponent)bindable;

            control.PageNumberChangedCommand.Execute(newValue);
        });
    public ICommand PageNumberChangedCommand
    {
        get => (ICommand)GetValue(PageNumberChangedCommandProperty);
        set => SetValue(PageNumberChangedCommandProperty, value);
    }
}