using ApacheKafkaWorker.Utils.Avros.Serializers;
using Confluent.Kafka;

namespace ApacheKafkaWorker.Utils.MessageBus
{
    public class KafkaMessageBus
    {
        private readonly string _bootstrapServers;

        public KafkaMessageBus(string bootstrapServers)
        {
            _bootstrapServers = bootstrapServers;
        }

        public async Task ProducerAsync<T>(string topic, T message)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers
            };

            var producer = new ProducerBuilder<string, T>(config)
                .SetValueSerializer(new AvroSerializer<T>())
                .Build();

            _ = await producer.ProduceAsync(topic, new Message<string, T>
            {
                Key = Guid.NewGuid().ToString(),
                Value = message
            });

            await Task.CompletedTask;
        }

        public async Task ConsumerAsync<T>(string topic, Func<T, Task> onMessage, CancellationToken cancellation)
        {
            _ = Task.Factory.StartNew(async () =>
            {
                var config = new ConsumerConfig
                {
                    GroupId = "string",
                    BootstrapServers = _bootstrapServers,
                    EnableAutoCommit = false,
                    EnableAutoOffsetStore = false
                };

                using var consumer = new ConsumerBuilder<string, T>(config)
                    .SetValueDeserializer(new AvroDeserializer<T>())
                    .Build();

                consumer.Subscribe(topic);

                while (!cancellation.IsCancellationRequested)
                {
                    var result = consumer.Consume();

                    if (result.IsPartitionEOF)
                    {
                        continue;
                    }

                    await onMessage(result.Message.Value);

                    consumer.Commit();
                }
            }, cancellation, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            await Task.CompletedTask;
        }
    }
}
