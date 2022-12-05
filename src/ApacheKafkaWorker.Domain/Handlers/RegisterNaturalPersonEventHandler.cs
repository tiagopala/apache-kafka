using ApacheKafkaWorker.Domain.Events;
using ApacheKafkaWorker.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApacheKafkaWorker.Domain.Handlers
{
    public class RegisterNaturalPersonEventHandler : IRequestHandler<RegisterNaturalPersonEvent>
    {
        private readonly ILogger<RegisterNaturalPersonEventHandler> _logger;
        private readonly INaturalPersonServices _naturalPersonServices;

        public RegisterNaturalPersonEventHandler(ILogger<RegisterNaturalPersonEventHandler> logger, INaturalPersonServices naturalPersonServices)
        {
            _logger = logger;
            _naturalPersonServices = naturalPersonServices;
        }

        public async Task<Unit> Handle(RegisterNaturalPersonEvent request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"User: {request.Id} received to be registered.");

            var message = new NaturalPersonCreatedEvent(request.Id, Guid.NewGuid().ToString());

            await _naturalPersonServices!.SendNaturalPersonCreatedEventAsync(message);

            _logger.LogInformation($"User: {request.Id} - CustomerId: {message.CustomerId} created sucessfully and sent to write off.");

            return Unit.Task.Result;
        }
    }
}