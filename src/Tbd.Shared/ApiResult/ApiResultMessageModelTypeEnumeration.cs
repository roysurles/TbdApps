namespace Tbd.Shared.ApiResult;

[JsonConverter(typeof(BaseEnumerationFromDisplayNameJsonConverter<ApiResultMessageModelTypeEnumeration>))]
public class ApiResultMessageModelTypeEnumeration : BaseEnumeration
{
    public static readonly ApiResultMessageModelTypeEnumeration Information = new(1, "Information");

    public static readonly ApiResultMessageModelTypeEnumeration Warning = new(2, "Warning");

    public static readonly ApiResultMessageModelTypeEnumeration Error = new(3, "Error");

    public static readonly ApiResultMessageModelTypeEnumeration UnhandledException = new(4, "UnhandledException");

    public static readonly ApiResultMessageModelTypeEnumeration Hidden = new(5, "Hidden");

    public static readonly ApiResultMessageModelTypeEnumeration Deprecated = new(6, "Deprecated");

    public ApiResultMessageModelTypeEnumeration() { }

    protected ApiResultMessageModelTypeEnumeration(int value, string displayName) : base(value, displayName) { }
}
