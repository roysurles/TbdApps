namespace Tbd.Shared.ApiLog
{
    public class ApiLoggingOptionsModel
    {
        public string ConnectionString { get; set; }

        public bool IsEnabled { get; set; }

        public int MinimumHttpStatusCode { get; set; } = 200;
    }
}
