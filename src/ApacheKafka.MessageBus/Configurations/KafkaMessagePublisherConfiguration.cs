using ApacheKafka.MessageBus.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace ApacheKafka.MessageBus.Configurations;

public static class KafkaMessagePublisherConfiguration
{
    public static IServiceCollection AddKafkaMessagePublisher(this IServiceCollection services, string bootstrapServers, string serviceName, string serviceVersion)
    {
        services.AddScoped<IKafkaMessagePublisher>(provider => new KafkaMessagePublisher(bootstrapServers, serviceName, serviceVersion));

        return services;
    }
}
