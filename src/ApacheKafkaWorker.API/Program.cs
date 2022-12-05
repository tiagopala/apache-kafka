using ApacheKafkaWorker.API;
using ApacheKafkaWorker.API.Tracing;
using Microsoft.AspNetCore.HttpLogging;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using System.Diagnostics;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// OpenTelemetry Configuration
builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder =>
{
    tracerProviderBuilder
        .AddSource(OpenTelemetryExtensions.ServiceName)
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName: OpenTelemetryExtensions.ServiceName, serviceVersion: OpenTelemetryExtensions.ServiceVersion))
        .AddHttpClientInstrumentation(options =>
        {
            options.EnrichWithHttpRequestMessage = (activity, request) =>
            {
                // TODO: Add request content body at tags.
            };
        })
        .AddAspNetCoreInstrumentation(options =>
        {
            options.EnrichWithHttpRequest = (activity, request) =>
            {
                // TODO: Add request content body at tags.
            };
        })
        .AddConsoleExporter() // Exportando também para o Console para capturar o traceId e pesquisar diretamente no jaeger
        .AddJaegerExporter(exporter =>
        {
            exporter.AgentHost = builder.Configuration["Jaeger:AgentHost"];
            exporter.AgentPort = Convert.ToInt32(builder.Configuration["Jaeger:AgentPort"]);
        });
});

// Serilog Configuration
builder.Host.UseSerilog((host, loggerConfiguration) => 
    loggerConfiguration
        .ReadFrom.Configuration(builder.Configuration));

// HTTP Logging for incoming request body
builder.Services.AddHttpLogging(logging => logging.LoggingFields = HttpLoggingFields.All);

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddServices(builder.Configuration);

var app = builder.Build();

// Enabling HTTP Logging
app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
