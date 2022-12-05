using ApacheKafkaWorker.Infrastructure.Avros;
using Confluent.Kafka;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace ApacheKafka.MessageBus.MessageBus;

public class KafkaMessagePublisher : IKafkaMessagePublisher
{
    private static readonly TextMapPropagator _propagator = Propagators.DefaultTextMapPropagator;

    private readonly string _bootstrapServers;
    private readonly string _serviceName;
    private readonly string _serviceVersion;

    public KafkaMessagePublisher(string bootstrapServers, string serviceName, string serviceVersion)
    {
        _bootstrapServers = bootstrapServers;
        _serviceName = serviceName;
        _serviceVersion = serviceVersion;
    }

    public async Task ProduceAsync<T>(string topicName, T message)
    {
        using var activity = new ActivitySource(_serviceName, _serviceVersion)
                .StartActivity($"{topicName}Send", ActivityKind.Producer);

        activity!.SetTag("messaging.payload", JsonSerializer.Serialize(message));

        var headers = new Headers
        {
            new Header("messaging.system", Encoding.UTF8.GetBytes("kafka")),
            new Header("messaging.operation", Encoding.UTF8.GetBytes("process")),
            new Header("messaging.destination", Encoding.UTF8.GetBytes(topicName)),
            new Header("messaging.destination_kind", Encoding.UTF8.GetBytes("topic"))
        };

        foreach (var header in headers)
        {
            activity!.SetTag(header.Key, Encoding.UTF8.GetString(header.GetValueBytes()));
        }

        _propagator.Inject(new PropagationContext(GetActivityContext(activity!), Baggage.Current), headers, (headers, key, value) => headers.Add(key, Encoding.UTF8.GetBytes(value)));

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

    private static ActivityContext GetActivityContext(Activity activity)
    {
        ActivityContext contextToInject = default;

        if (activity is not null)
        {
            contextToInject = activity.Context;
        }
        else if (Activity.Current is not null)
        {
            contextToInject = Activity.Current.Context;
        }

        return contextToInject;
    }
}
