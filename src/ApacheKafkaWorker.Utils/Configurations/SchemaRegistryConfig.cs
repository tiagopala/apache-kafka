namespace ApacheKafkaWorker.Utils.Configurations
{
    public class SchemaRegistryConfig : Confluent.SchemaRegistry.SchemaRegistryConfig
    {
        public SchemaRegistryConfig(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new NullReferenceException(nameof(url));

            base.Url = url;
        }
    }
}
