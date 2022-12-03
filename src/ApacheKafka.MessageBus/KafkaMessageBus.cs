using ApacheKafkaWorker.Infrastructure.Avros;
using Confluent.Kafka;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace ApacheKafka.MessageBus;

public class KafkaMessageBus : IKafkaMessageBus
{
    private readonly string _bootstrapServers;
    private readonly string _serviceName;
    private readonly string _serviceVersion;

    private readonly TextMapPropagator _textMapPropagator = Propagators.DefaultTextMapPropagator;

    public KafkaMessageBus(string bootstrapServers, string serviceName, string serviceVersion)
    {
        _bootstrapServers = bootstrapServers;
        _serviceName = serviceName;
        _serviceVersion = serviceVersion;
    }

    public async Task ProduceAsync<T>(string topicName, T message)
    {
        var activitySource = new ActivitySource(_serviceName, _serviceVersion);

        using var activity = activitySource.StartActivity($"{topicName}Send", ActivityKind.Producer);

        ActivityContext contextToInject = default;
        if (activity != null)
        {
            contextToInject = activity.Context;
        }
        else if (Activity.Current != null)
        {
            contextToInject = Activity.Current.Context;
        }

        var headers = new Headers();

        _textMapPropagator.Inject(new PropagationContext(contextToInject, Baggage.Current), headers, InjectTraceContextIntoHeaders);

        activity?.SetTag("messaging.system", "kafka");
        activity?.SetTag("messaging.destination_kind", "topic");
        activity?.SetTag("messaging.destination", topicName);
        activity?.SetTag("messaging.operation", "process");
        activity?.SetTag("message", JsonSerializer.Serialize(message));

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
