using Yape.Application.Transaction.Handlers;

namespace Yape.API.Transaction.Configurations
{
    public static class MediatRConfiguration
    {
        public static IServiceCollection AddMediatRService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateTransactionHandler>());
            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetTrasanctionByExternalIdHandler>());
            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetAllTransactionsHandler>());
            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<PatchTransanctionStatusHandler>());
            return serviceCollection;
        }
    }
}
