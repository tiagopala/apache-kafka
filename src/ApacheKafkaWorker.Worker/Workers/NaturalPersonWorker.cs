using ApacheKafka.MessageBus.BackgroundServices;
using ApacheKafkaWorker.Domain.Events;
using ApacheKafkaWorker.Worker.Tracing;

namespace ApacheKafkaWorker.Worker.Workers
{
    internal class NaturalPersonWorker : BaseKafkaWorker<RegisterNaturalPersonEvent>
    {
        public NaturalPersonWorker(IServiceProvider serviceProvider, ILogger<BaseKafkaWorker<RegisterNaturalPersonEvent>> logger, IConfiguration configuration)
            : base(serviceProvider,
                   logger, 
                   configuration["Kafka:BootstrapServers"], 
                   configuration["Kafka:Consumer:GroupId"], 
                   configuration["Kafka:TopicName"], 
                   OpenTelemetryExtensions.ServiceName,
                   OpenTelemetryExtensions.ServiceVersion) { }
    }
}
