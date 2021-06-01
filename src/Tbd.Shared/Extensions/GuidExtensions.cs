using System;

namespace Tbd.Shared.Extensions
{
    public static class GuidExtensions
    {
        public static Guid ToGuid(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return Guid.NewGuid();

            var wasParsed = Guid.TryParse(str, out Guid parsedGuid);

            return wasParsed
                ? parsedGuid
                : Guid.NewGuid();
        }
    }
}
