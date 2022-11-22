using ApacheKafkaWorker.Utils.Configurations;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var builder = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environment}.json", true, true);

var config = builder.Build();

var kafkaConfig = config.GetSection("ApacheKafkaConfig").Get<ApacheKafkaConfig>();

var producerConfig = new KafkaProducerConfig(kafkaConfig.BootstrapServers);

var kafkaProducer = new ProducerBuilder<string,string>(producerConfig).Build();

var random = new Random();

string topicName = kafkaConfig.TopicName ?? throw new NullReferenceException("Environment variable ApacheKafka:TopicName not set.");

var message = new Message<string, string>()
{
    Key = Guid.NewGuid().ToString(),
    Value = $"Message - {random.Next(1000)}"
};

var result = await kafkaProducer.ProduceAsync(topicName, message);

Console.WriteLine($"Message sent - offset: {result.Offset}");