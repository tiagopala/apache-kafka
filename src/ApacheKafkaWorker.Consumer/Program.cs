using ApacheKafkaWorker.Utils.Configurations;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var builder = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environment}.json", true, true);

var config = builder.Build();

var kafkaConfig = config.GetSection("ApacheKafkaConfig").Get<ApacheKafkaConfig>();

var consumerConfig = new KafkaConsumerConfig(kafkaConfig.BootstrapServers, kafkaConfig.Consumer.GroupId);

var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();

if (string.IsNullOrEmpty(kafkaConfig.TopicName))
    throw new NullReferenceException("Environment variable ApacheKafka:TopicName not set.");

consumer.Subscribe(kafkaConfig.TopicName);

Console.WriteLine("Consumer started.");

while (true)
{
    var result = consumer.Consume();

    Console.WriteLine($"Message received. Key: {result.Message.Key} - Value: {result.Message.Value}");
}