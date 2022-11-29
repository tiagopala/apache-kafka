using Microsoft.Extensions.DependencyInjection;

namespace ApacheKafka.MessageBus
{
    public static class KafkaMessageBusDependencyInjection
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services, string bootstrapServers)
        {
            if (bootstrapServers == null)
                throw new ArgumentNullException(nameof(bootstrapServers));

            services.AddSingleton<IKafkaMessageBus>(new KafkaMessageBus(bootstrapServers));

            return services;
        }
    }
}
