using Application.Handlers;

namespace Yape.Transaction.API.Configurations
{
    public static class MediatRConfiguration
    {
        public static IServiceCollection ConfigureMediatRService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateTransactionHandler>());
            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetTrasanctionByIdHandler>());
            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetAllTransactionsHandler>());
            return serviceCollection;
        }
    }
}
