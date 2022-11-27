using ApacheKafkaWorker.Domain.Commands;

namespace ApacheKafkaWorker.Domain.Services
{
    public interface ICreateNaturalPersonService
    {
        Task CreateNaturalPerson(CreateNaturalPersonCommand createNaturalPerson);
    }
}
