using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace ApacheKafkaWorker.Utils.Extensions
{
    public static class ClientConfigExtensions
    {
        public static void AddBootstrapServers(this ClientConfig config, string bootstrapServers)
        {
            if (string.IsNullOrEmpty(bootstrapServers))
                throw new NullReferenceException("Environment variable ApacheKafka:BootstrapServers not set.");

            config.BootstrapServers = bootstrapServers;
        }
    }
}
