namespace RecipeApp.Maui.Features.Shared.ValueChangedMessages;

public class IsBusyValueChangedMessage : ValueChangedMessage<bool>
{
    public IsBusyValueChangedMessage(bool value) : base(value) { }
}
