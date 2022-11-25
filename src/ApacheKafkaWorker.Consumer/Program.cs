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

Console.WriteLine("Consumer started.");

await messageBus.ConsumeAsync<Avros.Schemas.Event>(kafkaConfig.TopicName, async request => await ProcessMessage(request), new CancellationToken());

async Task ProcessMessage(Avros.Schemas.Event @event)
{
    Console.Write($"Message received. Id: {@event.Id}, Description: {@event.Description}.");
}
