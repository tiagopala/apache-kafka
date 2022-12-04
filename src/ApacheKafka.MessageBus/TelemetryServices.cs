using Confluent.Kafka;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace ApacheKafka.MessageBus
{
    public class TelemetryServices
    {
        public string ServiceName { get; }
        public string ServiceVersion { get; }

        private static readonly TextMapPropagator _textMapPropagator = Propagators.DefaultTextMapPropagator;

        public TelemetryServices(string serviceName, string serviceVersion)
        {
            ServiceName = serviceName;
            ServiceVersion = serviceVersion;
        }

        private (Activity, ActivityContext) AddNewActivity(string activityName, ActivityKind kind, Dictionary<string, string> tags)
        {
            using var activity = new ActivitySource(ServiceName, ServiceVersion)
                .StartActivity(activityName, kind);

            ActivityContext contextToInject = default;

            if (activity is not null)
            {
                contextToInject = activity.Context;
            }
            else if (Activity.Current is not null)
            {
                contextToInject = Activity.Current.Context;
            }

            if(tags.Any())
            {
                foreach (var tag in tags)
                {
                    activity?.SetTag(tag.Key, tag.Value);
                }
            }

            return (activity!, contextToInject);
        }

        public void AddKafkaProducerEventActivity<T>(string activityName, Headers headers, T messageBody)
        {
            var tags = new Dictionary<string, string>()
            {
                { "message", JsonSerializer.Serialize(messageBody) }
            };

            if (headers.Any())
            {
                foreach (var header in headers)
                {
                    tags.Add(header.Key, Encoding.UTF8.GetString(header.GetValueBytes()));
                }
            }

            var (activity, context) = AddNewActivity(activityName, ActivityKind.Producer, tags!);

            using (activity!)
            {
                _textMapPropagator.Inject(new PropagationContext(context, Baggage.Current), headers, InjectTraceContextIntoHeaders);
            };
        }

        private static void InjectTraceContextIntoHeaders(Headers headers, string key, string value)
            => headers.Add(key, Encoding.UTF8.GetBytes(value));
    }
}
