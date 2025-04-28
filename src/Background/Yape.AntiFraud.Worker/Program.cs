using Yape.AntiFraud.Worker;
using Yape.Domain.ApiClient;
using Yape.Domain.Message;
using Yape.Infrastructure.AntiFraudApi;
using Yape.Infrastructure.Kafka.AntiFraudMessage;
using Yape.Infrastructure.Kafka.TransactionMessage;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddScoped<IMessageConsumer, AntifraudMessageConsumer>();
builder.Services.AddScoped<IAntifraudApiClient, AntifraudApiClient>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();
