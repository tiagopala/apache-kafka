using System.Diagnostics;

namespace ApacheKafkaWorker.API.Tracing
{
    public static class OpenTelemetryExtensions
    {
        public static string ServiceName { get; }
        public static string ServiceVersion { get; }

        static OpenTelemetryExtensions()
        {
            ServiceName = typeof(OpenTelemetryExtensions).Assembly.GetName().Name!;
            ServiceVersion = typeof(OpenTelemetryExtensions).Assembly.GetName().Version!.ToString();
        }

        public static ActivitySource CreateActivitySource()
            => new(ServiceName, ServiceVersion);
    }
}
