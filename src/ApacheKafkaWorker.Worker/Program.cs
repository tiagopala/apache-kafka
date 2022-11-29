var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var builder = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environment}.json", true, true);

var config = builder.Build();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddServices(config);
    })
    .Build();

await host.RunAsync();
