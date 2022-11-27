using Microsoft.Extensions.DependencyInjection;

namespace ApacheKafka.MessageBus
{
    public static class KafkaMessageBusDependencyInjection
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services, string bootstrapServers)
        {
            services.AddSingleton<IKafkaMessageBus>(new KafkaMessageBus(bootstrapServers));

            return services;
        }
    }
}
