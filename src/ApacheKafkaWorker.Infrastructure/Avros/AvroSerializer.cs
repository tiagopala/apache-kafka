using Confluent.Kafka;
using System.Text.Json;

namespace ApacheKafkaWorker.Infrastructure.Avros
{
    public class AvroSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
            => JsonSerializer.SerializeToUtf8Bytes(data);
    }
}
