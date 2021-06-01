using System.Text.Json;

namespace Tbd.Shared.Extensions
{
    public static class JsonExtensions
    {
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
