using System.Text.Json.Serialization;

using Tbd.Shared.Enumeration;

namespace Tbd.Shared.OrderBy
{
    [JsonConverter(typeof(BaseEnumerationFromDisplayNameJsonConverter<OrderByDirectionEnumeration>))]
    public class OrderByDirectionEnumeration : BaseEnumeration
    {
        public static readonly OrderByDirectionEnumeration Ascending = new(0, "Asc");

        public static readonly OrderByDirectionEnumeration Descending = new(1, "Desc");

        public OrderByDirectionEnumeration() { }

        public OrderByDirectionEnumeration(int value, string displayName) : base(value, displayName) { }
    }
}
