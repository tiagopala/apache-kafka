using ApacheKafkaWorker.Infrastructure.Avros;
using Confluent.Kafka;
using System.Text;
using System.Text.Json;

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

        var headers = new Headers();

        var tags = new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("messaging.system", "kafka"),
            new KeyValuePair<string, string>("messaging.destination_kind", "topic"),
            new KeyValuePair<string, string>("messaging.destination", topicName),
            new KeyValuePair<string, string>("messaging.operation", "process"),
            new KeyValuePair<string, string>("message", JsonSerializer.Serialize(message))
        };

        telemetryServices.AddProducerEventActivity(
            $"{topicName}Send",
            headers,
            InjectTraceContextIntoHeaders, tags);

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

    private void InjectTraceContextIntoHeaders(Headers headers, string key, string value)
        => headers.Add(key, Encoding.UTF8.GetBytes(value));

}
