namespace ApacheKafka.MessageBus
{
    public interface IKafkaMessageBus
    {
        Task ProduceAsync<T>(string topic, T message, string application, string traceId = "");
    }
}