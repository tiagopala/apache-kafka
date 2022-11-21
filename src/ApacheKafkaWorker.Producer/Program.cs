using Confluent.Kafka;

var kafkaConfig = new ProducerConfig() { BootstrapServers = "localhost:9092" };

var kafkaProducer = new ProducerBuilder<string,string>(kafkaConfig).Build();

var random = new Random();

const string topicName = "topic-test";

var message = new Message<string, string>()
{
    Key = Guid.NewGuid().ToString(),
    Value = $"Message - {random.Next(1000)}"
};

var result = await kafkaProducer.ProduceAsync(topicName, message);

Console.WriteLine($"Message sent - offset: {result.Offset}");