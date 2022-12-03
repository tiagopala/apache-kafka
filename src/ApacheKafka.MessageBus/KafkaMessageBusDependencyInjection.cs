using Microsoft.Extensions.DependencyInjection;

namespace ApacheKafka.MessageBus
{
    public static class KafkaMessageBusDependencyInjection
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services, string bootstrapServers, string serviceName, string serviceVersion)
        {
            services.AddSingleton<IKafkaMessageBus>(new KafkaMessageBus(bootstrapServers, serviceName, serviceVersion));

            return services;
        }
    }
}
