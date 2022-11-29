using ApacheKafkaWorker.Domain.Commands;
using ApacheKafkaWorker.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApacheKafkaWorker.Domain.Handlers
{
    public class CreateNaturalPersonCommandHandler : IRequestHandler<CreateNaturalPersonCommand, string>
    {
        private readonly ILogger<CreateNaturalPersonCommandHandler> _logger;
        private readonly INaturalPersonServices _naturalPersonServices;

        public CreateNaturalPersonCommandHandler(ILogger<CreateNaturalPersonCommandHandler> logger, INaturalPersonServices naturalPersonServices)
        {
            _naturalPersonServices = naturalPersonServices;
            _logger = logger;
        }

        public async Task<string> Handle(CreateNaturalPersonCommand request, CancellationToken cancellationToken)
        {
            // TODO: Salvar na tabela 

            // Enviar evento para serviço de criação de usuário ✅
            await _naturalPersonServices.SendCreateNaturalPersonEventAsync(request);

            _logger.LogInformation($"User: {request.Id} sent to be created");

            return request.Id;
        }
    }
}
