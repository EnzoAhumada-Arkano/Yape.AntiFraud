using Yape.Application.Antifraud.Handlers;

namespace Yape.API.AntiFraud.Configurations
{
    public static class MediatRConfiguration
    {
        public static IServiceCollection AddMediatRService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ValidateTransactionHandler>());
            return serviceCollection;
        }
    }
}
