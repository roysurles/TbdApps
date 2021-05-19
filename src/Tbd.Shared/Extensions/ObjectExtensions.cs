using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

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

        public static string SerializeToJson<T>(this T obj, JsonSerializerOptions options = null) =>
            JsonSerializer.Serialize(obj, options);

        public static T DeserializeFromJson<T>(this string json, JsonSerializerOptions options = null) =>
            JsonSerializer.Deserialize<T>(json, options);

        public static T DeepClone<T>(this T source, JsonSerializerOptions serializeOptions = null, JsonSerializerOptions deserializeOptions = null)
        {
            // Don't serialize a null object, simply return the default for that object
            if (source == null)
                return default;

            return JsonSerializer.Deserialize<T>(source.SerializeToJson(serializeOptions), deserializeOptions);
        }
    }
}
