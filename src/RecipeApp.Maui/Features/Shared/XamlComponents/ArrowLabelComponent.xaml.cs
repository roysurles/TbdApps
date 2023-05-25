namespace RecipeApp.Maui.Features.Shared.XamlComponents;

public partial class ArrowLabelComponent : Label
{
    public ArrowLabelComponent()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty IsExpandedProperty =
        BindableProperty.Create(nameof(IsExpanded), typeof(bool?), typeof(ArrowLabelComponent), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (ArrowLabelComponent)bindable;
        if ((bool?)newValue is null)
            control.Text = string.Empty;
        if ((bool?)newValue == true)
            control.Text = "˄";
        if ((bool?)newValue == false)
            control.Text = "˅";
    });
    public bool? IsExpanded
    {
        get => GetValue(IsExpandedProperty) as bool?;
        set => SetValue(IsExpandedProperty, value);
    }
}