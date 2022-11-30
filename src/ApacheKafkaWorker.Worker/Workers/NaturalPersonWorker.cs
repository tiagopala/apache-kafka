using ApacheKafka.MessageBus.BackgroundServices;
using ApacheKafkaWorker.Domain.Events;
using MediatR;

namespace ApacheKafkaWorker.Worker.Workers
{
    internal class NaturalPersonWorker : BaseKafkaWorker<RegisterNaturalPersonEvent>
    {
        public NaturalPersonWorker(ILogger<BaseKafkaWorker<RegisterNaturalPersonEvent>> logger, IConfiguration configuration, IMediator mediator)
            : base(logger, mediator, configuration["Kafka:BootstrapServers"], configuration["Kafka:Consumer:GroupId"], configuration["Kafka:TopicName"]) { }
    }
}
