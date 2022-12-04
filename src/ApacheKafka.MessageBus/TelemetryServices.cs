using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;

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

        private (Activity, ActivityContext) AddNewActivity(string activityName, ActivityKind kind, params KeyValuePair<string, string>[] tags)
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

            foreach (var tag in tags)
            {
                activity?.SetTag(tag.Key, tag.Value);
            }

            return (activity!, contextToInject);
        }

        public void AddProducerEventActivity<T>(string activityName, T generic, Action<T, string, string> action, params KeyValuePair<string, string>[] tags)
        {
            var (activity, context) = AddNewActivity(activityName, ActivityKind.Producer, tags);

            using (activity!)
            {
                _textMapPropagator.Inject(new PropagationContext(context, Baggage.Current), generic, action);
            };
        }
    }
}
