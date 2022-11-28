using ApacheKafkaWorker.Domain.Commands;
using ApacheKafkaWorker.Domain.Services;
using MediatR;

namespace ApacheKafkaWorker.Domain.Handlers
{
    public class CreateNaturalPersonCommandHandler : IRequestHandler<CreateNaturalPersonCommand, string>
    {
        private readonly INaturalPersonServices _naturalPersonServices;

        public CreateNaturalPersonCommandHandler(INaturalPersonServices naturalPersonServices)
        {
            _naturalPersonServices = naturalPersonServices;
        }

        public async Task<string> Handle(CreateNaturalPersonCommand request, CancellationToken cancellationToken)
        {
            // TODO: Salvar na tabela 

            // Enviar evento para serviço de criação de usuário ✅
            await _naturalPersonServices.SendCreateNaturalPersonEventAsync(request);

            return request.Id;
        }
    }
}
