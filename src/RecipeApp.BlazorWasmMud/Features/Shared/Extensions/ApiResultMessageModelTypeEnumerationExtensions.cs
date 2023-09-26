namespace RecipeApp.BlazorWasmMud.Features.Shared.Extensions;

public static class ApiResultMessageModelTypeEnumerationExtensions
{
    public static MudBlazor.Severity ToSeverity(this ApiResultMessageModelTypeEnumeration apiResultMessageModelTypeEnumeration)
    {
        /*
            Information
            Warning
            Error
            UnhandledException
            Hidden
            Deprecated
        */

        var severity = MudBlazor.Severity.Info;  // TODO:  default; probably need a better mapping
        if (apiResultMessageModelTypeEnumeration.Equals(ApiResultMessageModelTypeEnumeration.Information))
            severity = MudBlazor.Severity.Info;
        if (apiResultMessageModelTypeEnumeration.Equals(ApiResultMessageModelTypeEnumeration.Error))
            severity = MudBlazor.Severity.Error;
        if (apiResultMessageModelTypeEnumeration.Equals(ApiResultMessageModelTypeEnumeration.UnhandledException))
            severity = MudBlazor.Severity.Error;
        if (apiResultMessageModelTypeEnumeration.Equals(ApiResultMessageModelTypeEnumeration.Warning))
            severity = MudBlazor.Severity.Warning;

        return severity;
    }
}
