
namespace RecipeApp.Maui;

public interface ISampleViewModel
{
    ObservableCollection<string> Items { get; set; }
    void IncrementCounter();
}

public partial class SampleViewModel : ObservableObject, ISampleViewModel
{
    [ObservableProperty]
    ObservableCollection<string> items = new();

    [RelayCommand]
    public void IncrementCounter()
    {
    }
}
