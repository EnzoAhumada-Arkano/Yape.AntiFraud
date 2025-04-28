using Yape.Transaction.Worker;
using Yape.Transaction.Worker.Configurations;

var builder = Host.CreateApplicationBuilder(args);
ServiceConfiguration.AddServiceConfiguration(builder.Services);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();
