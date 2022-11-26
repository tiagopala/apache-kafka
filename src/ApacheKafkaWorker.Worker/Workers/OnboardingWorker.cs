using ApacheKafkaWorker.Domain.Events;
using MediatR;

namespace ApacheKafkaWorker.Worker.Workers
{
    internal class OnboardingWorker : BaseKafkaWorker<OnboardingEvent>
    {
        public OnboardingWorker(ILogger<BaseKafkaWorker<OnboardingEvent>> logger, IConfiguration configuration, IMediator mediator) : base(logger, configuration, mediator) { }
    }
}
