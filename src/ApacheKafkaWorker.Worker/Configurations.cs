using ApacheKafkaWorker.Domain.Events;
using ApacheKafkaWorker.Domain.Handlers;
using ApacheKafkaWorker.Worker.Workers;
using MediatR;

namespace ApacheKafkaWorker.Worker
{
    internal static class Configurations
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            _ = services
                .AddHostedService<OnboardingWorker>()
                .AddMediatR(typeof(OnboardingEvent))
                .RegisterHandlers();

            return services;
        }

        public static IServiceCollection RegisterHandlers(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<OnboardingEvent>, CreateOnboardingEventHandler>();

            return services;
        }
    }
}
