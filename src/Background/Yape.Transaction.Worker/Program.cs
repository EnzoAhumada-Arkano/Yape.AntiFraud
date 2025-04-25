using Yape.Domain.ApiClient;
using Yape.Domain.Message;
using Yape.Infrastructure.Kafka.TransactionMessage;
using Yape.Infrastructure.TransactionApi;
using Yape.Transaction.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddScoped<IMessageConsumer, TransactionMessageConsumer>();
builder.Services.AddScoped<ITransactionApiClient, TransactionApiClient>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();
