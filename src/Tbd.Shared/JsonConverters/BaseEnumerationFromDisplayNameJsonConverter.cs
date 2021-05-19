using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using Tbd.Shared.Enumeration;

namespace Tbd.Shared.JsonConverters
{
    public class BaseEnumerationFromDisplayNameJsonConverter<TEnumeration> : JsonConverter<TEnumeration> where TEnumeration : BaseEnumeration, new()
    {
        public override TEnumeration Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            BaseEnumeration.FromDisplayName<TEnumeration>(reader.GetString());

        public override void Write(Utf8JsonWriter writer, TEnumeration value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.DisplayName);
    }
}
