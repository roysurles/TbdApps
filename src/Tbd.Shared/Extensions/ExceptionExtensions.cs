using System;
using System.Text;

namespace Tbd.Shared.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetExceptionData(this Exception ex, int innerExceptionCount = 0)
        {
            if (ex is null)
                return null;

            // TODO:  Get data from AggregateException.InnerExceptions (plural)
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("Source: ")
                .AppendLine(ex.Source)
                .Append(ex.GetType().FullName)
                .Append(": ")
                .AppendLine(ex.Message)
                .AppendLine(ex.StackTrace);

            if (ex.InnerException != null)
            {
                innerExceptionCount++;
                stringBuilder.AppendLine()
                    .Append("InnerException ")
                    .Append(innerExceptionCount)
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(ex.InnerException.GetExceptionData(innerExceptionCount));
            }

            return stringBuilder.ToString();
        }
    }
}
