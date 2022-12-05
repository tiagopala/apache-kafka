using ApacheKafka.MessageBus.MessageBus;
using ApacheKafkaWorker.Domain.Commands;
using ApacheKafkaWorker.Domain.Events;
using ApacheKafkaWorker.Domain.Services;

namespace ApacheKafkaWorker.Infrastructure.Services
{
    public class NaturalPersonServices : INaturalPersonServices
    {
        private readonly IKafkaMessagePublisher _messageBus;

        public NaturalPersonServices(IKafkaMessagePublisher messageBus)
        {
            _messageBus = messageBus;
        }

        public async Task SendCreateNaturalPersonEventAsync(CreateNaturalPersonCommand createNaturalPerson)
            => await _messageBus.ProduceAsync("CreateUser", createNaturalPerson);

        public async Task SendNaturalPersonCreatedEventAsync(NaturalPersonCreatedEvent personCreatedEvent)
            => await _messageBus.ProduceAsync("UserCreated", personCreatedEvent);
    }
}
