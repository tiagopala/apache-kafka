using ApacheKafka.MessageBus;
using ApacheKafkaWorker.Domain.Commands;
using ApacheKafkaWorker.Domain.Events;
using ApacheKafkaWorker.Domain.Handlers;
using ApacheKafkaWorker.Domain.Services;
using ApacheKafkaWorker.Infrastructure.Services;
using ApacheKafkaWorker.Worker.Workers;
using MediatR;

namespace ApacheKafkaWorker.Worker
{
    internal static class Configurations
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services
                .AddMessageBus(configuration["Kafka:BootstrapServers"])
                .AddMediatR(typeof(RegisterNaturalPersonEvent))
                .RegisterHostedServices()
                .RegisterServices()
                .RegisterHandlers();

            return services;
        }

        public static IServiceCollection RegisterHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<NaturalPersonWorker>();

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ICreateNaturalPersonService, CreateNaturalPersonService>();

            return services;
        }

        public static IServiceCollection RegisterHandlers(this IServiceCollection services)
        {
            services
                .AddScoped<IRequestHandler<RegisterNaturalPersonEvent>, RegisterNaturalPersonEventHandler>()
                .AddScoped<IRequestHandler<CreateNaturalPersonCommand, string>, CreateNaturalPersonCommandHandler>();

            return services;
        }
    }
}
