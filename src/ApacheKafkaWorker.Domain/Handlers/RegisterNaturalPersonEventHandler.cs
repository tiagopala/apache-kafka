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
            var message = new NaturalPersonCreatedEvent(request.Id, Guid.NewGuid().ToString());

            await _naturalPersonServices.SendNaturalPersonCreatedEventAsync(message);

            _logger.LogInformation("Processed");

            return Unit.Task.Result;
        }
    }
}