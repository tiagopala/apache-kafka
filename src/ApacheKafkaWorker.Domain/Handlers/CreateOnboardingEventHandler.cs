using ApacheKafkaWorker.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApacheKafkaWorker.Domain.Handlers
{
    public class CreateOnboardingEventHandler : IRequestHandler<OnboardingEvent>
    {
        private readonly ILogger<CreateOnboardingEventHandler> _logger;

        public CreateOnboardingEventHandler(ILogger<CreateOnboardingEventHandler> logger)
        {
            _logger = logger;
        }

        public Task<Unit> Handle(OnboardingEvent request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processed");

            return Unit.Task;
        }
    }
}
