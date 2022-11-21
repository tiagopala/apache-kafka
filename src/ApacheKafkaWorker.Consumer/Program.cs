using Confluent.Kafka;

var kafkaConfig = new ConsumerConfig() { GroupId = "group-1", BootstrapServers = "localhost:9092" };

var consumer = new ConsumerBuilder<string, string>(kafkaConfig).Build();

consumer.Subscribe("topic-test");

Console.WriteLine("Consumer started.");

while (true)
{
    var result = consumer.Consume();

    Console.WriteLine($"Message received. Key: {result.Message.Key} - Value: {result.Message.Value}");
}