namespace RecipeApp.Maui.Features.Shared.XamlComponents;

public partial class ArrowLabelComponent : Label
{
    public ArrowLabelComponent()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty IsExpandedProperty =
        BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(ArrowLabelComponent), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (ArrowLabelComponent)bindable;
        if (newValue is null)
            control.Text = string.Empty;
        if ((bool)newValue)
            control.Text = "˄";
        if (!(bool)newValue)
            control.Text = "˅";
    });
    public bool? IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }
}