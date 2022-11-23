using ApacheKafkaWorker.Utils.Configurations;
using ApacheKafkaWorker.Utils.MessageBus;
using Microsoft.Extensions.Configuration;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var builder = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environment}.json", true, true);

var config = builder.Build();

var kafkaConfig = config.GetSection("ApacheKafkaConfig").Get<ApacheKafkaConfig>();

var messageBus = new KafkaMessageBus(kafkaConfig.BootstrapServers);

string topicName = kafkaConfig.TopicName ?? throw new NullReferenceException(nameof(kafkaConfig.TopicName));

var message = new Avros.Schemas.Event
{
    Id = Guid.NewGuid().ToString(),
    Description = "Event description."
};

await messageBus.ProducerAsync(topicName, message);

Console.WriteLine($"Message sent.");