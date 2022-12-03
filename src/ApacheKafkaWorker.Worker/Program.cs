using ApacheKafkaWorker.Worker.Tracing;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    // Serilog Configuration
    .UseSerilog((host, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(host.Configuration))
    .ConfigureServices((hostContext, services) =>
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true)
            .AddEnvironmentVariables();

        // OpenTelemetry Configuration
        services.AddOpenTelemetryTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .AddSource(OpenTelemetryExtensions.ServiceName)
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(serviceName: OpenTelemetryExtensions.ServiceName, serviceVersion: OpenTelemetryExtensions.ServiceVersion))
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                //.AddConsoleExporter() // Remoção do exporter para o console visto que estamos utilizando o exporter para o jaeger abaixo
                .AddJaegerExporter(exporter =>
                {
                    exporter.AgentHost = hostContext.Configuration["Jaeger:AgentHost"];
                    exporter.AgentPort = Convert.ToInt32(hostContext.Configuration["Jaeger:AgentPort"]);
                });
        });

        services.AddServices(hostContext.Configuration);
    })
    .Build();

await host.RunAsync();
