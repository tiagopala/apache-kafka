using Confluent.Kafka;
using System.IO.Compression;
using System.Text.Json;

namespace ApacheKafkaWorker.Utils.Avros.Serializers
{
    public class AvroDeserializer<T> : IDeserializer<T>
    {
        // Sem compressão:
        //public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        //    => JsonSerializer.Deserialize<T>(data);
        
        // Com compressão:
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            using var memoryStream = new MemoryStream(data.ToArray());
            using var zipStream = new GZipStream(memoryStream, CompressionMode.Decompress, true);
            return JsonSerializer.Deserialize<T>(zipStream);
        }
    }
}
