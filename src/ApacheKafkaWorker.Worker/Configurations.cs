using ApacheKafka.MessageBus;
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
                .AddMessageBus(configuration["Kafka:BootstrapServers"]!, typeof(Program).Assembly.GetName().Name!, typeof(Program).Assembly.GetName().Version!.ToString())
                .AddMediatR(typeof(RegisterNaturalPersonEvent))
                .RegisterServices()
                .RegisterHandlers()
                .RegisterHostedServices();

            return services;
        }

        public static IServiceCollection RegisterHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<NaturalPersonWorker>();

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<INaturalPersonServices, NaturalPersonServices>();

            return services;
        }

        public static IServiceCollection RegisterHandlers(this IServiceCollection services)
        {
            services
                .AddScoped<IRequestHandler<RegisterNaturalPersonEvent>, RegisterNaturalPersonEventHandler>();

            return services;
        }
    }
}
