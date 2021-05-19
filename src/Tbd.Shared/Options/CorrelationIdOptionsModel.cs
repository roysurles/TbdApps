namespace Tbd.Shared.Options
{
    public class CorrelationIdOptionsModel
    {
        /// <summary>
        /// The header field name where the correlation ID will be stored.
        /// </summary>
        public string Header { get; set; } = "X-Correlation-ID";

        /// <summary>
        /// <para>
        /// Controls whether the correlation ID is returned in the response headers.
        /// </para>
        /// <para>Default: true</para>
        /// </summary>
        public bool IncludeInResponse { get; set; } = true;

        /// <summary>
        /// <para>
        /// Controls whether the ASP.NET Core TraceIdentifier will be set to match the CorrelationId.
        /// </para>
        /// <para>Default: true</para>
        /// </summary>
        public bool UpdateTraceIdentifier { get; set; } = true;

        /// <summary>
        /// <para>
        /// Controls whether a GUID will be used in cases where no correlation ID is retrieved from the request header.
        /// When false the TraceIdentifier for the current request will be used.
        /// </para>
        /// <para> Default: false.</para>
        /// </summary>
        public bool UseGuidForCorrelationId { get; set; }
    }
}
