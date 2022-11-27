using ApacheKafkaWorker.Infrastructure.Avros;
using Confluent.Kafka;
using MediatR;
using System.Text;
using System.Text.Json;

namespace ApacheKafkaWorker.Worker.Workers;

public abstract class BaseKafkaWorker<T> : BackgroundService
{
    private readonly ILogger<BaseKafkaWorker<T>> _logger;
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator;

    protected BaseKafkaWorker(ILogger<BaseKafkaWorker<T>> logger, IConfiguration configuration, IMediator mediator)
    {
        _logger = logger;
        _configuration = configuration;
        _mediator = mediator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumerConfig = new ConsumerConfig()
        {
            BootstrapServers = _configuration["Kafka:BootstrapServers"],
            GroupId = _configuration["Kafka:Consumer:GroupId"],
            EnableAutoCommit = false,
            EnableAutoOffsetStore = true,
        };

        using var consumer = new ConsumerBuilder<string, T>(consumerConfig)
            .SetValueDeserializer(new AvroDeserializer<T>())
            .Build();

        consumer.Subscribe(_configuration["Kafka:TopicName"]);

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
            }

            _logger.Log(LogLevel.Information, $"Message '{result.Message.Key}' consumed.");

            consumer.Commit();
        }
    }

    private string GetTraceId(Dictionary<string, string> headers)
    {
        if (!headers.TryGetValue("trace-id", out string traceId))
            traceId = Guid.NewGuid().ToString();

        return traceId;
    }
}