
using System.Text.Json.Serialization;

using Tbd.Shared.Enumeration;
using Tbd.Shared.JsonConverters;

namespace Tbd.Shared.OrderBy
{
    [JsonConverter(typeof(BaseEnumerationFromDisplayNameJsonConverter<OrderByTypeEnumeration>))]
    public class OrderByTypeEnumeration : BaseEnumeration
    {
        public static readonly OrderByTypeEnumeration Ascending = new(1, "Ascending");

        public static readonly OrderByTypeEnumeration Descending = new(2, "Descending");

        public OrderByTypeEnumeration() { }

        protected OrderByTypeEnumeration(int value, string displayName) : base(value, displayName) { }
    }
}
