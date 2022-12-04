using ApacheKafka.MessageBus.BackgroundServices;
using ApacheKafkaWorker.API.Tracing;
using ApacheKafkaWorker.Domain.Events;
using MediatR;

namespace ApacheKafkaWorker.API.BackgroundServices
{
    public class NaturalPersonCreatedBackgroundService : BaseKafkaWorker<NaturalPersonCreatedEvent>
    {
        public NaturalPersonCreatedBackgroundService(ILogger<BaseKafkaWorker<NaturalPersonCreatedEvent>> logger, IConfiguration configuration, IMediator mediator)
            : base(logger,
                   mediator,
                   configuration["Kafka:BootstrapServers"]!,
                   configuration["Kafka:Consumer:GroupId"]!,
                   configuration["Kafka:TopicName"]!,
                   OpenTelemetryExtensions.ServiceName,
                   OpenTelemetryExtensions.ServiceVersion)
        { }
    }
}
