using ApacheKafkaWorker.Domain.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ApacheKafkaWorker.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NaturalPersonController : ControllerBase
    {
        private readonly ILogger<NaturalPersonController> _logger;
        private readonly IMediator _mediator;

        public NaturalPersonController(ILogger<NaturalPersonController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateNaturalPersonCommand cmd)
        {
            // TODO: Validate command using FluentValidation

            _logger.LogInformation($"Create natural person requested. Payload: {JsonSerializer.Serialize(cmd)}");

            var response = await _mediator.Send(cmd);

            return Ok(new { UserId = response });
        }
    }
}