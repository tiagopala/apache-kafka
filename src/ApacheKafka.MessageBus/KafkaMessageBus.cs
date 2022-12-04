using ApacheKafkaWorker.Infrastructure.Avros;
using Confluent.Kafka;
using System.Text;

namespace ApacheKafka.MessageBus;

public class KafkaMessageBus : IKafkaMessageBus
{
    private readonly string _bootstrapServers;
    private readonly string _serviceName;
    private readonly string _serviceVersion;

    public KafkaMessageBus(string bootstrapServers, string serviceName, string serviceVersion)
    {
        _bootstrapServers = bootstrapServers;
        _serviceName = serviceName;
        _serviceVersion = serviceVersion;
    }

    public async Task ProduceAsync<T>(string topicName, T message)
    {
        var telemetryServices = new TelemetryServices(_serviceName, _serviceVersion);

        var headers = new Headers
        {
            new Header("messaging.system", Encoding.UTF8.GetBytes("kafka")),
            new Header("messaging.destination_kind", Encoding.UTF8.GetBytes("topic")),
            new Header("messaging.destination", Encoding.UTF8.GetBytes(topicName)),
            new Header("messaging.operation", Encoding.UTF8.GetBytes("process"))
        };

        telemetryServices.AddKafkaProducerEventActivity($"{topicName}Send", headers, message);

        var config = new ProducerConfig
        {
            BootstrapServers = _bootstrapServers
        };

        var producer = new ProducerBuilder<string, T>(config)
            .SetValueSerializer(new AvroSerializer<T>())
            .Build();

        _ = await producer.ProduceAsync(topicName, new Message<string, T>
        {
            Key = Guid.NewGuid().ToString(),
            Headers = headers,
            Value = message
        });

        await Task.CompletedTask;
    }
}
