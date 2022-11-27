using ApacheKafka.MessageBus;
using ApacheKafkaWorker.Domain.Commands;
using ApacheKafkaWorker.Domain.Handlers;
using ApacheKafkaWorker.Domain.Services;
using ApacheKafkaWorker.Infrastructure.Services;
using MediatR;

namespace ApacheKafkaWorker.API
{
    public static class Configurations
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services
                .AddMessageBus(configuration["Kafka:BootstrapServers"])
                .AddMediatR(typeof(CreateNaturalPersonCommand))
                .AddScoped<ICreateNaturalPersonService, CreateNaturalPersonService>()
                .AddScoped<IRequestHandler<CreateNaturalPersonCommand, string>, CreateNaturalPersonCommandHandler>();

            return services;
        }
    }
}
