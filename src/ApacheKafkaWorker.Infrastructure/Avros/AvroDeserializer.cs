using Confluent.Kafka;
using System.Text.Json;

namespace ApacheKafkaWorker.Infrastructure.Avros
{
    public class AvroDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
            => JsonSerializer.Deserialize<T>(data);
    }
}
