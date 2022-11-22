using ApacheKafkaWorker.Utils.Extensions;

namespace ApacheKafkaWorker.Utils.Configurations
{
    public class KafkaProducerConfig : Confluent.Kafka.ProducerConfig
    {
        public KafkaProducerConfig(string bootstrapServers)
            => this.AddBootstrapServers(bootstrapServers);
    }
}
