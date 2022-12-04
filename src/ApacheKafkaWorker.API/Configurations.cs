using ApacheKafka.MessageBus.Configurations;
using ApacheKafkaWorker.API.BackgroundServices;
using ApacheKafkaWorker.Domain.Commands;
using ApacheKafkaWorker.Domain.Events;
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
                .AddKafkaMessagePublisher(configuration["Kafka:BootstrapServers"]!, typeof(Program).Assembly.GetName().Name!, typeof(Program).Assembly.GetName().Version!.ToString())
                .AddMediatR(typeof(CreateNaturalPersonCommand))
                .AddScoped<INaturalPersonServices, NaturalPersonServices>()
                .AddScoped<IRequestHandler<NaturalPersonCreatedEvent>, NaturalPersonCreatedEventHandler>()
                .AddScoped<IRequestHandler<CreateNaturalPersonCommand, string>, CreateNaturalPersonCommandHandler>()
                .AddHostedService<NaturalPersonCreatedBackgroundService>();

            return services;
        }
    }
}
