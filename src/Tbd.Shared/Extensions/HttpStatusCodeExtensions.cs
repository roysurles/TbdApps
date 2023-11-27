namespace Tbd.Shared.Extensions;

public static class HttpStatusCodeExtensions
{
    public static int ToInt(this HttpStatusCode httpStatusCode) =>
        (int)httpStatusCode;
}
