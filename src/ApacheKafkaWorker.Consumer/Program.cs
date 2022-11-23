using ApacheKafkaWorker.Utils.Configurations;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var builder = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environment}.json", true, true);

var config = builder.Build();

var schemaRegistryConfig = new ApacheKafkaWorker.Utils.Configurations.SchemaRegistryConfig(config["SchemaRegistryConfig:Url"]);

var schemaRegistryClient = new CachedSchemaRegistryClient(schemaRegistryConfig);

var kafkaConfig = config.GetSection("ApacheKafkaConfig").Get<ApacheKafkaConfig>();

var consumerConfig = new KafkaConsumerConfig(kafkaConfig.BootstrapServers, kafkaConfig.Consumer.GroupId);

var consumer = new ConsumerBuilder<string, Avros.Schemas.Event>(consumerConfig)
    .SetValueDeserializer(new AvroDeserializer<Avros.Schemas.Event>(schemaRegistryClient)
    .AsSyncOverAsync())
    .Build();

if (string.IsNullOrEmpty(kafkaConfig.TopicName))
    throw new ArgumentNullException(paramName: nameof(kafkaConfig.TopicName));

consumer.Subscribe(kafkaConfig.TopicName);

Console.WriteLine("Consumer started.");

while (true)
{
    var result = consumer.Consume();

    var message = result.Message.Value;

    Console.WriteLine($"Message received. Key: {result.Message.Key} - Event Id: {result.Message.Value.Id} Event Description: {result.Message.Value.Description}");
}