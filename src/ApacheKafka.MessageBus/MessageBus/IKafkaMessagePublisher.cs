namespace ApacheKafka.MessageBus.MessageBus;

public interface IKafkaMessagePublisher
{
    Task ProduceAsync<T>(string topicName, T message);
}