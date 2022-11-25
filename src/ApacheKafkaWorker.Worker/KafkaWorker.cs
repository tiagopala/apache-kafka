using Confluent.Kafka;

namespace ApacheKafkaWorker.Worker;

public class KafkaWorker : BackgroundService
{
    private readonly ILogger<KafkaWorker> _logger;
    private readonly IConfiguration _configuration;

    public KafkaWorker(ILogger<KafkaWorker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
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

        while (!stoppingToken.IsCancellationRequested)
        {
            // Terminar de implementar consumo
            
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}