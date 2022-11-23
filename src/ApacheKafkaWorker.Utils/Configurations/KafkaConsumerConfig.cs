using ApacheKafkaWorker.Utils.Extensions;

namespace ApacheKafkaWorker.Utils.Configurations
{
    public class KafkaConsumerConfig : Confluent.Kafka.ConsumerConfig
    {
        public KafkaConsumerConfig(string bootstrapServers, string groupId)
        {
            this.AddBootstrapServers(bootstrapServers);

            if (string.IsNullOrEmpty(groupId))
                throw new ArgumentNullException(nameof(groupId));

            base.GroupId = groupId;
        }
    }
}
