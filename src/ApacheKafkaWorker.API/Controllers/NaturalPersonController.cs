using ApacheKafkaWorker.API.Tracing;
using ApacheKafkaWorker.Domain.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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

            using var activity = OpenTelemetryExtensions.CreateActivitySource()
                .StartActivity("PersonCreateRequested");

            ActivityContext contextToInject = default;

            if (activity is not null)
            {
                contextToInject = activity.Context;
            } 
            else if (Activity.Current is not null)
            {
                contextToInject = Activity.Current.Context;
            }

            activity?.SetTag("payload", JsonSerializer.Serialize(cmd));

            _logger.LogInformation($"Create natural person requested. Payload: {JsonSerializer.Serialize(cmd)}");

            var response = await _mediator.Send(cmd);

            return Ok(new { UserId = response });
        }
    }
}