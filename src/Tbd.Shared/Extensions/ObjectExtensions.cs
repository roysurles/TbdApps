using System;
using System.Diagnostics.CodeAnalysis;

namespace Tbd.Shared.Extensions
{
    public static class ObjectExtensions
    {
        [SuppressMessage("Simplification", "RCS1084:Use coalesce expression instead of conditional expression.", Justification = "<Pending>")]
        public static object IsNullToDbNull(this object obj) =>
            obj is null ? DBNull.Value : obj;

        public static string ToActualValueAsString(this object obj)
        {
            return obj is null
                ? "[NULL]"
                : string.IsNullOrWhiteSpace(obj?.ToString())
                    ? "[EMPTY STRING]"
                    : obj?.ToString();
        }
    }
}
