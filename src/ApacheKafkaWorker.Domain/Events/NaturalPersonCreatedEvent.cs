using MediatR;

namespace ApacheKafkaWorker.Domain.Events
{
    public class NaturalPersonCreatedEvent : IRequest
    {
        public NaturalPersonCreatedEvent(string userId, string customerId)
        {
            UserId = userId;
            CustomerId = customerId;
        }

        public string UserId { get; set; }
        public string CustomerId { get; set; }
    }
}
