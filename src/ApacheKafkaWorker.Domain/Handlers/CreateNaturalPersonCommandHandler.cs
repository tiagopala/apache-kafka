using ApacheKafkaWorker.Domain.Commands;
using ApacheKafkaWorker.Domain.Services;
using MediatR;

namespace ApacheKafkaWorker.Domain.Handlers
{
    public class CreateNaturalPersonCommandHandler : IRequestHandler<CreateNaturalPersonCommand, string>
    {
        private readonly ICreateNaturalPersonService _createNaturalPersonService;

        public CreateNaturalPersonCommandHandler(ICreateNaturalPersonService createNaturalPersonService)
        {
            _createNaturalPersonService = createNaturalPersonService;
        }

        public async Task<string> Handle(CreateNaturalPersonCommand request, CancellationToken cancellationToken)
        {
            // TODO: Salvar na tabela 

            // Enviar evento para serviço de criação de usuário ✅
            await _createNaturalPersonService.CreateNaturalPerson(request);

            return request.Id;
        }
    }
}
