namespace ApacheKafkaWorker.Utils.Configurations
{
    public class ApacheKafkaConfig
    {
        public ConsumerConfig Consumer { get; set; }
        public string BootstrapServers { get; set; }
        public string TopicName { get; set; }

    }

    public class ConsumerConfig
    {
        public string GroupId { get; set; }
    }
}
