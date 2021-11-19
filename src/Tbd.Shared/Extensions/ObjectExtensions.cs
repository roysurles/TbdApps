using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Tbd.Shared.Extensions
{
    public static class ObjectExtensions
    {
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

        /// <summary>
        /// This method is ~28 nanoseconds more than $"{nameof(object)}.{nameof(member)}"
        /// 1 millisecond = 1,000,000 nanoseconds
        /// 1 second = 1,000 milliseconds
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="callerMemberName"></param>
        /// <returns></returns>
        public static string GetObjectAndMemberName(this object obj, [CallerMemberName] string callerMemberName = null)
        {
            var objName = obj.GetType().Name;
            return $"{objName}.{callerMemberName}";
        }
    }
}
