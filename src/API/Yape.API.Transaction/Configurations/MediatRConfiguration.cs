using Yape.Application.Transaction.Handlers;

namespace Yape.Transaction.API.Configurations
{
    public static class MediatRConfiguration
    {
        public static IServiceCollection ConfigureMediatRService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateTransactionHandler>());
            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetTrasanctionByExternalIdHandler>());
            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetAllTransactionsHandler>());
            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<PatchTransanctionStatusHandler>());
            return serviceCollection;
        }
    }
}
