using System;

namespace Tbd.Shared.ApiLog
{
    public class ApiLogDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string ConnectionId { get; set; }

        public string TraceId { get; set; }

        public string Claims { get; set; }

        public string LocalIpAddress { get; set; }

        public string RemoteIpAddress { get; set; }

        public string AssemblyName { get; set; }

        public string Url { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public DateTimeOffset ActionDateTimeOffset { get; set; } = DateTimeOffset.Now;

        public string HttpProtocol { get; set; }

        public string HttpMethod { get; set; }

        public int HttpStatusCode { get; set; }

        public string ExceptionData { get; set; }

        public long ElapsedMilliseconds { get; set; }

        public string HttpRequestBody { get; set; }

        public string HttpResponseBody { get; set; }
    }
}
