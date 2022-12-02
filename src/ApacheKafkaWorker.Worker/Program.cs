using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;



IHost host = Host.CreateDefaultBuilder(args)
    // Serilog Configuration
    .UseSerilog((host, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(host.Configuration))
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;

        var builder = new ConfigurationBuilder()
            .SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true)
            .AddEnvironmentVariables();

        // OpenTelemetry Configuration
        services.AddOpenTelemetryTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .AddConsoleExporter()
                .AddSource(ApplicationExtensions.ServiceName)
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(serviceName: ApplicationExtensions.ServiceName, serviceVersion: ApplicationExtensions.ServiceVersion))
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation();
        });

        services.AddServices(configuration);
    })
    .Build();

await host.RunAsync();
