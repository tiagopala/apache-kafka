using ApacheKafkaWorker.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApacheKafkaWorker.Domain.Handlers
{
    public class NaturalPersonCreatedEventHandler : IRequestHandler<NaturalPersonCreatedEvent>
    {
        private ILogger<NaturalPersonCreatedEventHandler> _logger;

        public NaturalPersonCreatedEventHandler(ILogger<NaturalPersonCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task<Unit> Handle(NaturalPersonCreatedEvent request, CancellationToken cancellationToken)
        {
            // TODO: Update UserId Status

            // TODO: Send Email Customer Created

            _logger.LogInformation($"User {request.UserId} created. CustomerId: {request.CustomerId}");

            return Unit.Task;
        }
    }
}
