using ApacheKafka.MessageBus;
using ApacheKafkaWorker.API.BackgroundServices;
using ApacheKafkaWorker.Domain.Commands;
using ApacheKafkaWorker.Domain.Events;
using ApacheKafkaWorker.Domain.Handlers;
using ApacheKafkaWorker.Domain.Services;
using ApacheKafkaWorker.Infrastructure.Services;
using MediatR;
using Serilog;

namespace ApacheKafkaWorker.API
{
    public static class Configurations
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services
                .AddMessageBus(configuration["Kafka:BootstrapServers"])
                .AddMediatR(typeof(CreateNaturalPersonCommand))
                .AddScoped<INaturalPersonServices, NaturalPersonServices>()
                .AddScoped<IRequestHandler<NaturalPersonCreatedEvent>, NaturalPersonCreatedEventHandler>()
                .AddScoped<IRequestHandler<CreateNaturalPersonCommand, string>, CreateNaturalPersonCommandHandler>()
                .AddHostedService<NaturalPersonCreatedBackgroundService>();

            return services;
        }

        public static void AddSerilog(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}
