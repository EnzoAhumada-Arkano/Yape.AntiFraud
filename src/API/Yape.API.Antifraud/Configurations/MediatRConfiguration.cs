using Yape.Application.Antifraud.Handlers;

namespace Yape.Antifraud.API.Configurations
{
    public static class MediatRConfiguration
    {
        public static IServiceCollection ConfigureMediatRService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ValidateTransactionHandler>());
            return serviceCollection;
        }
    }
}
