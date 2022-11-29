using ApacheKafkaWorker.Infrastructure.Avros;
using Confluent.Kafka;
using System.Text;

namespace ApacheKafka.MessageBus
{
    public class KafkaMessageBus : IKafkaMessageBus
    {
        private readonly string _bootstrapServers;

        public KafkaMessageBus(string bootstrapServers)
        {
            _bootstrapServers = bootstrapServers;
        }

        public async Task ProduceAsync<T>(string topic, T message, string application, string traceId = "")
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers
            };

            var producer = new ProducerBuilder<string, T>(config)
                .SetValueSerializer(new AvroSerializer<T>())
                .Build();

            var headers = new Headers
            {
                new Header("application", Encoding.UTF8.GetBytes(application)),
                new Header("trace-id", string.IsNullOrEmpty(traceId) ? Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()) : Encoding.UTF8.GetBytes(traceId))
            };

            _ = await producer.ProduceAsync(topic, new Message<string, T>
            {
                Key = Guid.NewGuid().ToString(),
                Headers = headers,
                Value = message
            });

            await Task.CompletedTask;
        }
    }
}
