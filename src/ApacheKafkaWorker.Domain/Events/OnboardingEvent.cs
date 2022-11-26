using MediatR;

namespace ApacheKafkaWorker.Domain.Events
{
    public class OnboardingEvent : IRequest
    {
        public string Id { get; set; }
        public string Description { get; set; }
    }
}
