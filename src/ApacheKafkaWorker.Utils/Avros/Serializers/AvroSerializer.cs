using Confluent.Kafka;
using System.IO.Compression;
using System.Text.Json;

namespace ApacheKafkaWorker.Utils.Avros.Serializers
{
    public class AvroSerializer<T> : ISerializer<T>
    {
        // Sem compressão:
        //public byte[] Serialize(T data, SerializationContext context)
        // => JsonSerializer.SerializeToUtf8Bytes(data);

        // Com compressão:
        public byte[] Serialize(T data, SerializationContext context)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(data);
            using var memoryStream = new MemoryStream();
            using var zipStream = new GZipStream(memoryStream, CompressionMode.Compress, true);
            zipStream.Write(bytes, 0, bytes.Length);
            zipStream.Close();
            var buffer = memoryStream.ToArray();
            return buffer;
        }
    }
}
