using ApacheKafkaWorker.Utils.Configurations;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Configuration;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var builder = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environment}.json", true, true);

var config = builder.Build();

var kafkaConfig = config.GetSection("ApacheKafkaConfig").Get<ApacheKafkaConfig>();

var schemaRegistryConfig = new ApacheKafkaWorker.Utils.Configurations.SchemaRegistryConfig(config["SchemaRegistryConfig:Url"]);

var schemaClient = new CachedSchemaRegistryClient(schemaRegistryConfig);

var producerConfig = new KafkaProducerConfig(kafkaConfig.BootstrapServers);

var kafkaProducer = new ProducerBuilder<string,Avros.Schemas.Event>(producerConfig)
    .SetValueSerializer(new AvroSerializer<Avros.Schemas.Event>(schemaClient))
    .Build();

string topicName = kafkaConfig.TopicName ?? throw new NullReferenceException(nameof(kafkaConfig.TopicName));

var message = new Message<string, Avros.Schemas.Event>()
{
    Key = Guid.NewGuid().ToString(),
    Value = new Avros.Schemas.Event
    {
        Id = Guid.NewGuid().ToString(),
        Description = "Event description."
    }
};

var result = await kafkaProducer.ProduceAsync(topicName, message);

Console.WriteLine($"Message sent - offset: {result.Offset}");