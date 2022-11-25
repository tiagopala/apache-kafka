namespace ApacheKafkaWorker.Worker
{
    internal static class Configurations
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHostedService<KafkaWorker>();

            return services;
        }
    }
}
