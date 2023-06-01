namespace Tbd.Shared.Extensions;

public static class StringExtensions
{
    /// <summary>
    ///     A string extension method that query if '@this' is numeric.
    ///     https://csharp-extension.com/en/method/1002061/string-isnumeric
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if numeric, false if not.</returns>
    public static bool IsNumeric(this string str) =>
        decimal.TryParse(str, out _);
}