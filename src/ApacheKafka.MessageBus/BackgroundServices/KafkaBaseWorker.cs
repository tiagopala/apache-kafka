using ApacheKafkaWorker.Infrastructure.Avros;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace ApacheKafka.MessageBus.BackgroundServices
{
    public abstract class BaseKafkaWorker<T> : BackgroundService where T : IRequest
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TextMapPropagator _textMapPropagator = Propagators.DefaultTextMapPropagator;
        private readonly ILogger<BaseKafkaWorker<T>> _logger;
        private readonly string _bootstrapServers;
        private readonly string _groupId;
        private readonly string _topicName;
        private readonly string _serviceName;
        private readonly string _serviceVersion;

        protected BaseKafkaWorker(IServiceProvider serviceProvider, ILogger<BaseKafkaWorker<T>> logger, string bootstrapServers, string groupId, string topicName, string serviceName, string serviceVersion)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _bootstrapServers = bootstrapServers;
            _groupId = groupId;
            _topicName = topicName;
            _serviceName = serviceName;
            _serviceVersion = serviceVersion;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _ = Task.Factory.StartNew(async () =>
            {
                var consumerConfig = new ConsumerConfig()
                {
                    BootstrapServers = _bootstrapServers,
                    GroupId = _groupId,
                    EnableAutoCommit = false,
                    EnableAutoOffsetStore = true,
                };

                using var consumer = new ConsumerBuilder<string, T>(consumerConfig)
                    .SetValueDeserializer(new AvroDeserializer<T>())
                    .Build();

                consumer.Subscribe(_topicName);

                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(stoppingToken);

                    if (result.IsPartitionEOF)
                    {
                        continue;
                    }

                    try
                    {
                        var parentContext = _textMapPropagator.Extract(default, result.Message.Headers, ExtractTraceContextFromHeaders);
                        Baggage.Current = parentContext.Baggage;

                        var activitySource = new ActivitySource(_serviceName, _serviceVersion);

                        using var activity = activitySource.StartActivity($"{_topicName}Received", ActivityKind.Consumer, parentContext.ActivityContext);

                        ActivityContext contextToInject = default;
                        if (activity != null)
                        {
                            contextToInject = activity.Context;
                        }
                        else if (Activity.Current != null)
                        {
                            contextToInject = Activity.Current.Context;
                        }

                        var message = result.Message.Value;

                        var serializedMessage = JsonSerializer.Serialize(message);

                        activity?.SetTag("messaging.system", "kafka");
                        activity?.SetTag("messaging.destination_kind", "topic");
                        activity?.SetTag("messaging.destination", _topicName);
                        activity?.SetTag("messaging.operation", "process");
                        activity?.SetTag("messaging.kafka.consumer_group", _groupId);
                        activity?.SetTag("messaging.kafka.partition", result.Partition.ToString());
                        activity?.SetTag("message", serializedMessage);

                        _logger.Log(LogLevel.Information, $"Message received. Payload: {serializedMessage}");

                        var headers = result.Message.Headers.ToDictionary(h => h.Key, h => Encoding.UTF8.GetString(h.GetValueBytes()));

                        using (IServiceScope scope = _serviceProvider.CreateScope())
                        {
                            IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                            await mediator.Send(message);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.Log(LogLevel.Error, e, $"Message '{result.Message.Key}' could not be processed.");

                        throw;
                    }

                    _logger.Log(LogLevel.Information, $"Message '{result.Message.Key}' consumed.");

                    consumer.Commit();
                }
            }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            await Task.CompletedTask;
        }

        private static IEnumerable<string> ExtractTraceContextFromHeaders(Headers headers, string key)
        {
            var header = headers.FirstOrDefault(h => h.Key == key);

            if (header is not null)
                return new[] { Encoding.UTF8.GetString(header.GetValueBytes()) };

            return Enumerable.Empty<string>();
        }
    }
}
