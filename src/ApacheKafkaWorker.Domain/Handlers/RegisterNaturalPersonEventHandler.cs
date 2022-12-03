using ApacheKafkaWorker.Domain.Events;
using ApacheKafkaWorker.Domain.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ApacheKafkaWorker.Domain.Handlers
{
    public class RegisterNaturalPersonEventHandler : IRequestHandler<RegisterNaturalPersonEvent>
    {
        private readonly ILogger<RegisterNaturalPersonEventHandler> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RegisterNaturalPersonEventHandler(ILogger<RegisterNaturalPersonEventHandler> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<Unit> Handle(RegisterNaturalPersonEvent request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"User: {request.Id} received to be registered.");

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var naturalPersonServices = scope.ServiceProvider.GetService<INaturalPersonServices>();

                var message = new NaturalPersonCreatedEvent(request.Id, Guid.NewGuid().ToString());

                await naturalPersonServices!.SendNaturalPersonCreatedEventAsync(message);

                _logger.LogInformation($"User: {request.Id} - CustomerId: {message.CustomerId} created sucessfully and sent to write off.");
            }

            return Unit.Task.Result;
        }
    }
}