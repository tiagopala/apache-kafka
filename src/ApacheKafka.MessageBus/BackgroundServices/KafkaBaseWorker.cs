﻿using ApacheKafkaWorker.Infrastructure.Avros;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace ApacheKafka.MessageBus.BackgroundServices
{
    public abstract class BaseKafkaWorker<T> : BackgroundService where T : IRequest
    {
        private readonly ILogger<BaseKafkaWorker<T>> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly string _topicName;

        protected BaseKafkaWorker(ILogger<BaseKafkaWorker<T>> logger, IConfiguration configuration, IMediator mediator, string topicName)
        {
            _logger = logger;
            _configuration = configuration;
            _mediator = mediator;
            _topicName = topicName;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _ = Task.Factory.StartNew(async () =>
            {
                var consumerConfig = new ConsumerConfig()
                {
                    BootstrapServers = _configuration["Kafka:BootstrapServers"] ?? throw new ArgumentNullException("Kafka:BootstrapServers"),
                    GroupId = _configuration["Kafka:Consumer:GroupId"] ?? throw new ArgumentNullException("Kafka:Consumer:GroupId"),
                    EnableAutoCommit = false,
                    EnableAutoOffsetStore = true,
                };

                using var consumer = new ConsumerBuilder<string, T>(consumerConfig)
                    .SetValueDeserializer(new AvroDeserializer<T>())
                    .Build();

                consumer.Subscribe(_topicName ?? throw new ArgumentNullException("Kafka:BootstrapServers"));

                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(stoppingToken);

                    if (result.IsPartitionEOF)
                    {
                        continue;
                    }

                    try
                    {
                        _logger.Log(LogLevel.Information, $"Message received. Payload: {JsonSerializer.Serialize(result.Message.Value)}");

                        var headers = result.Message.Headers.ToDictionary(h => h.Key, h => Encoding.UTF8.GetString(h.GetValueBytes()));

                        var traceId = GetTraceId(headers);

                        var message = result.Message.Value;

                        await _mediator.Send(message);

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

        private static string GetTraceId(Dictionary<string, string> headers)
        {
            if (!headers.TryGetValue("trace-id", out string traceId))
                traceId = Guid.NewGuid().ToString();

            return traceId;
        }
    }
}
