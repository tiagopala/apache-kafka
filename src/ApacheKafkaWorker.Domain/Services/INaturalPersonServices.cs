using ApacheKafkaWorker.Domain.Commands;
using ApacheKafkaWorker.Domain.Events;

namespace ApacheKafkaWorker.Domain.Services
{
    public interface INaturalPersonServices
    {
        Task SendCreateNaturalPersonEventAsync(CreateNaturalPersonCommand createNaturalPerson);
        Task SendNaturalPersonCreatedEventAsync(NaturalPersonCreatedEvent personCreatedEvent);
    }
}
