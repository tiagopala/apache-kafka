namespace ApacheKafka.MessageBus;

public interface IKafkaMessageBus
{
    Task ProduceAsync<T>(string topicName, T message);
}