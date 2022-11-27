using ApacheKafka.MessageBus;
using ApacheKafkaWorker.Domain.Commands;
using ApacheKafkaWorker.Domain.Services;

namespace ApacheKafkaWorker.Infrastructure.Services
{
    public class CreateNaturalPersonService : ICreateNaturalPersonService
    {
        private readonly IKafkaMessageBus _messageBus;

        public CreateNaturalPersonService(IKafkaMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public async Task CreateNaturalPerson(CreateNaturalPersonCommand createNaturalPerson)
        {
            await _messageBus.ProduceAsync("create-user", createNaturalPerson, "ApacheKafkaWorker.API");
        }
    }
}
