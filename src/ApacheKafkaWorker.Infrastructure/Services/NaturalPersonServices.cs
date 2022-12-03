using ApacheKafka.MessageBus;
using ApacheKafkaWorker.Domain.Commands;
using ApacheKafkaWorker.Domain.Events;
using ApacheKafkaWorker.Domain.Services;

namespace ApacheKafkaWorker.Infrastructure.Services
{
    public class NaturalPersonServices : INaturalPersonServices
    {
        private readonly IKafkaMessageBus _messageBus;

        public NaturalPersonServices(IKafkaMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public async Task SendCreateNaturalPersonEventAsync(CreateNaturalPersonCommand createNaturalPerson)
            => await _messageBus.ProduceAsync("create-user", createNaturalPerson);

        public async Task SendNaturalPersonCreatedEventAsync(NaturalPersonCreatedEvent personCreatedEvent)
            => await _messageBus.ProduceAsync("user-created", personCreatedEvent);
    }
}
