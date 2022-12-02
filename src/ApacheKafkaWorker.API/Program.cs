using ApacheKafkaWorker.API;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog Configuration
builder.Host.UseSerilog((host, loggerConfiguration) => 
    loggerConfiguration
        .ReadFrom.Configuration(builder.Configuration));

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddServices(builder.Configuration);

var app = builder.Build();

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
