using ApacheKafka.MessageBus;
using ApacheKafkaWorker.Producer;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMessageBus(builder.Configuration["Kafka:BootstrapServers"]);

var app = builder.Build();

var messageBus = app.Services.GetService<IKafkaMessageBus>();

var message = new UserEvent("", "", "", 0, 0, new Address("", 0));

await messageBus.ProduceAsync(builder.Configuration["Kafka:TopicName"], message, "ApacheKafkaWorker.Producer");

Console.WriteLine($"Message sent.");