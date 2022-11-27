using ApacheKafkaWorker.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApacheKafkaWorker.Domain.Handlers
{
    public class RegisterNaturalPersonEventHandler : IRequestHandler<RegisterNaturalPersonEvent>
    {
        private readonly ILogger<RegisterNaturalPersonEventHandler> _logger;

        public RegisterNaturalPersonEventHandler(ILogger<RegisterNaturalPersonEventHandler> logger)
        {
            _logger = logger;
        }

        public Task<Unit> Handle(RegisterNaturalPersonEvent request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processed");

            return Unit.Task;
        }
    }
}
