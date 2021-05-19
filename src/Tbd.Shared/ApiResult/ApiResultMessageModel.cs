
using System.Text.Json.Serialization;

namespace Tbd.Shared.ApiResult
{
    /// <summary>
    /// Represents a message about the current result.
    /// </summary>
    public class ApiResultMessageModel : IApiResultMessageModel
    {
        /// <summary>
        /// Represents the severity or level of this message.
        /// </summary>
        [JsonPropertyName("messageType")]
        public ApiResultMessageModelTypeEnumeration MessageType { get; set; }

        /// <summary>
        /// Represents a code for this message; Usually will equate to a HttpStatusCode, but could be different.
        /// </summary>
        [JsonPropertyName("code")]
        public int? Code { get; set; }

        /// <summary>
        /// Friendly message about this message.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// Source that caused this message.
        /// </summary>
        [JsonPropertyName("source")]
        public string Source { get; set; }

        ///// <summary>
        ///// UnhandledException
        ///// </summary>
        //[JsonProperty("unhandledException")]
        //public Exception UnhandledException { get; set; }

        // TODO
        //public bool ShouldSerializeUnhandledException() =>
        //    !StaticServices.HostingEnvironment.IsProduction();
    }

    /// <summary>
    /// Represents a message about the current result.
    /// </summary>
    public interface IApiResultMessageModel
    {
        /// <summary>
        /// Represents the severity or level of this message.
        /// </summary>
        ApiResultMessageModelTypeEnumeration MessageType { get; set; }

        /// <summary>
        /// Represents a code for this message; Usually will equate to a HttpStatusCode, but could be different.
        /// </summary>
        int? Code { get; set; }

        /// <summary>
        /// Friendly message about this message.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Source that caused this message.
        /// </summary>
        string Source { get; set; }

        ///// <summary>
        ///// UnhandledException
        ///// </summary>
        //Exception UnhandledException { get; set; }

        //bool ShouldSerializeUnhandledException();
    }
}
