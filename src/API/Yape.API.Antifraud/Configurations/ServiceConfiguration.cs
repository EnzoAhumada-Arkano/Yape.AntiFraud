using Yape.Domain.Repository;
using Yape.Infrastructure.Postgresql.Repository;

namespace Yape.API.AntiFraud.Configurations
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddServiceConfiguration(this IServiceCollection services)
        {
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            return services;
        }
    }
}
