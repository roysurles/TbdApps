using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using Tbd.Shared.Enumeration;

namespace Tbd.Shared.JsonConverters
{
    public class BaseEnumerationFromValueJsonConverter<TEnumeration> : JsonConverter<TEnumeration> where TEnumeration : BaseEnumeration, new()
    {
        public override TEnumeration Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            _ = int.TryParse(reader.GetString(), out int value);
            return BaseEnumeration.FromValue<TEnumeration>(value);
        }

        public override void Write(Utf8JsonWriter writer, TEnumeration value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.Value.ToString());
    }
}
